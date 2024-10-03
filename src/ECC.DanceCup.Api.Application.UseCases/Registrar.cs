using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Application.UseCases;

public static class Registrar
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Registrar).Assembly);
        });
        
        services.AddValidatorsFromAssembly(typeof(Registrar).Assembly);
        
        return services;
    }
}