using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Storage;

public static class Registrar
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        
        services.AddScoped<IDanceViewRepository, DanceViewRepository>();

        services.Configure<StorageOptions>(configuration.GetSection(nameof(StorageOptions)));
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var connectionString = configuration.GetSection(nameof(StorageOptions))[nameof(StorageOptions.ConnectionString)];
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder =>
            {
                runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Registrar).Assembly);
            });
        
        return services;
    }
}