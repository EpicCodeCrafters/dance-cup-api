using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace ECC.DanceCup.Api.Presentation.Grpc;

public static class Registrar
{
    public static IEndpointRouteBuilder UseGrpcServices(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGrpcService<DanceCupGrpcService>();

        return endpointRouteBuilder;
    }
}