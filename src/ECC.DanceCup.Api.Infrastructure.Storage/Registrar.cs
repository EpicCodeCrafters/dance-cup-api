using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.Storage;

public static class Registrar
{
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        return services;
    }
}