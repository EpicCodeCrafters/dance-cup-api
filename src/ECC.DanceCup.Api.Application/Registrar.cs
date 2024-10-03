using ECC.DanceCup.Api.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Application;

public static class Registrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddUseCases();
        
        return services;
    }
}