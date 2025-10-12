using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using ECC.DanceCup.Api.Infrastructure.TgApi.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Infrastructure.TgApi;

public static class Registrar
{
    public static IServiceCollection AddTgApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TgApiOptions>()
            .Bind(configuration.GetSection(nameof(TgApiOptions)));
        services.AddScoped<ITgApi, TgApi>();
        
        return services;
    }
}