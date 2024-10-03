using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;
using ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Storage;

public static class Registrar
{
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        
        services.AddScoped<IDanceViewRepository, DanceViewRepository>();
        
        return services;
    }
}