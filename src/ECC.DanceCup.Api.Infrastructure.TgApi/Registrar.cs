using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.TgApi;

public static class Registrar
{
    public static IServiceCollection AddTgApi(this IServiceCollection services)
    {
        services.AddScoped<ITgApi, TgApi>();
        
        return services;
    }
}