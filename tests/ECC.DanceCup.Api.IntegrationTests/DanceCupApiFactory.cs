using Confluent.Kafka;
using Confluent.Kafka.Admin;
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
    
    public string PostgresConnectionString => _postgres.GetConnectionString();
    public string KafkaBoostrapServers => NormalizeBootstrapServers(_kafka.GetBootstrapAddress());
    
    public string RedisConnectionString => _redis.GetConnectionString();
    
    private string Environment => "Testing";
    
    private Dictionary<string, string?> Settings => new()
    {
        ["StorageOptions:ConnectionString"] = PostgresConnectionString,
        ["KafkaOptions:BootstrapServers"] = KafkaBoostrapServers,
        ["CachingOptions:ConnectionString"] = RedisConnectionString
    };

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
            .UseEnvironment(Environment)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(Settings);
            });

        builder.ConfigureServices(services =>
        {
        });
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await Task.WhenAll(
            _postgres.StartAsync(),
            _kafka.StartAsync(),
            _redis.StartAsync()
        );
        
        // Migrate database

        var migrateHost = WebHost
            .CreateDefaultBuilder()
            .UseStartup<Startup>()
            .UseEnvironment(Environment)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(Settings);
            })
            .Build();

        await migrateHost.MigrateAsync();
        
        // Init Kafka topics
        
        var adminConfig = new AdminClientConfig { BootstrapServers = KafkaBoostrapServers };
        using var adminClient = new AdminClientBuilder(adminConfig).Build();

        await adminClient.CreateTopicsAsync([
            new TopicSpecification
            {
                Name = "dance_cup_events",
                NumPartitions = 1,
                ReplicationFactor = 1
            }
        ]);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Task.WhenAll(
            _postgres.DisposeAsync().AsTask(),
            _kafka.DisposeAsync().AsTask(),
            _redis.DisposeAsync().AsTask()
        );
    }
    
    private static string NormalizeBootstrapServers(string address)
    {
        const string plaintextPrefix = "PLAINTEXT://";

        if (address.StartsWith(plaintextPrefix, StringComparison.OrdinalIgnoreCase))
        {
            address = address[plaintextPrefix.Length..];
        }

        if (address[^1] == '/')
        {
            address = address[..^1];
        }

        return address;
    }
}