using ECC.DanceCup.Api.Application.Abstractions.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Metrics;

public static class Registrar
{
    public static IServiceCollection AddCustomMetrics(this IServiceCollection services)
    {
        services.AddSingleton<IMetrics, CustomMetrics>();
        
        return services;
    }
}