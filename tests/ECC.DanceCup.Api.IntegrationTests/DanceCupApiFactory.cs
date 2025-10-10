using ECC.DanceCup.Api.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace ECC.DanceCup.Api.IntegrationTests;

public class DanceCupApiFactory : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithDatabase("dance-cup-api")
        .WithUsername("postgres-user")
        .WithPassword("postgres")
        .WithImage("postgres:15-alpine")
        .WithCleanUp(true)
        .Build();

    private readonly KafkaContainer _kafka = new KafkaBuilder()
        .WithImage("confluentinc/cp-kafka:7.5.0")
        .WithCleanUp(true)
        .Build();
        
    private readonly RedisContainer _redis = new RedisBuilder()
        .WithImage("redis:7-alpine")
        .WithCleanUp(true)
        .Build();

    protected override IHostBuilder CreateHostBuilder()
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Development")
            .ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["StorageOptions:ConnectionString"] = _postgres.GetConnectionString(),
                    ["KafkaOptions:BootstrapServers"] = _kafka.GetBootstrapAddress(),
                    ["CachingOptions:ConnectionString"] = _redis.GetConnectionString()
                };
                config.AddInMemoryCollection(settings);
            });

        builder.ConfigureServices(services =>
        {
            // Например, заменить реальные зависимости на тестовые / мок-версии
            // services.Remove(...);
            // services.AddSingleton<…>(…);
        });
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await Task.WhenAll(
            _postgres.StartAsync(),
            _kafka.StartAsync(),
            _redis.StartAsync()
        );

        var migrateHost = WebHost
            .CreateDefaultBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Development")
            .ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["StorageOptions:ConnectionString"] = _postgres.GetConnectionString(),
                    ["KafkaOptions:BootstrapServers"] = _kafka.GetBootstrapAddress(),
                    ["CachingOptions:ConnectionString"] = _redis.GetConnectionString()
                };
                config.AddInMemoryCollection(settings);
            })
            .Build();

        await migrateHost.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Task.WhenAll(
            _postgres.DisposeAsync().AsTask(),
            _kafka.DisposeAsync().AsTask(),
            _redis.DisposeAsync().AsTask()
        );
    }
}