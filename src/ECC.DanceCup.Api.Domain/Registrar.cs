using ECC.DanceCup.Api.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Domain;

public static class Registrar
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ITournamentFactory, TournamentFactory>();
        
        return services;
    }
}