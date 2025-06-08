using ECC.DanceCup.Api.Application.Abstractions.Caching;
using ECC.DanceCup.Api.Infrastructure.Caching.Options;
using ECC.DanceCup.Api.Infrastructure.Caching.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Caching;

public static class Registrar
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["CachingOptions:ConnectionString"];
        });
        
        services.AddScoped<IDanceViewCache, DanceViewCache>();
        
        services.Configure<CachingOptions>(configuration.GetSection("CachingOptions"));

        return services;
    }
}