using ECC.DanceCup.Api.Presentation.Grpc.Interceptors;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Presentation.Grpc;

public static class Registrar
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ValidationInterceptor>();
        });
        
        services.AddValidatorsFromAssembly(typeof(Registrar).Assembly);
        
        return services;
    }
    
    public static IEndpointRouteBuilder UseGrpcServices(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGrpcService<DanceCupApiGrpcService>();

        return endpointRouteBuilder;
    }
}