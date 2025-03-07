using ECC.DanceCup.Api.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Domain;

public static class Registrar
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IRefereeFactory, RefereeFactory>();
        services.AddTransient<ITournamentFactory, TournamentFactory>();
        services.AddTransient<IUserFactory, UserFactory>();
        
        return services;
    }
}