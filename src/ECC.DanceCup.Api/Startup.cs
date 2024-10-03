using ECC.DanceCup.Api.Application;
using ECC.DanceCup.Api.Domain;
using ECC.DanceCup.Api.Infrastructure.Storage;
using ECC.DanceCup.Api.Infrastructure.TgApi;
using ECC.DanceCup.Api.Presentation.Grpc;

namespace ECC.DanceCup.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        services.AddApplicationServices();

        services.AddStorage();
        services.AddTgApi();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.UseGrpcServices();
        });
    }
}