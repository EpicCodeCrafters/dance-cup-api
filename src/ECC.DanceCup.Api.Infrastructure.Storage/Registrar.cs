using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.Providers;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using ECC.DanceCup.Api.Infrastructure.Storage.Providers;
using ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Storage;

public static class Registrar
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRefereeRepository, RefereeRepository>();
        // TODO Сделать scoped
        services.AddSingleton<ITournamentRepository, TournamentRepository>();

        services.AddScoped<ICoupleIdProvider, CoupleIdProvider>();

        services.AddScoped<IDanceViewRepository, DanceViewRepository>();

        services.AddScoped<IRefereeViewRepository, RefereeViewRepository>();

        services.AddTransient<IPostgresConnectionFactory, PostgresConnectionFactory>();

        services.Configure<StorageOptions>(configuration.GetSection("StorageOptions"));

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var connectionString = configuration["StorageOptions:ConnectionString"];
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