using ECC.DanceCup.Api.Application;
using ECC.DanceCup.Api.Domain;
using ECC.DanceCup.Api.Infrastructure.Caching;
using ECC.DanceCup.Api.Infrastructure.Storage;
using ECC.DanceCup.Api.Infrastructure.TgApi;
using ECC.DanceCup.Api.Presentation.Grpc;
using ECC.DanceCup.Api.Presentation.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECC.DanceCup.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        services.AddApplicationServices();

        services.AddStorage(_configuration);
        services.AddCaching(_configuration);
        services.AddTgApi(_configuration);

        services.AddGrpcServices();
        services.AddGrpcHealthChecks().AddCheck(string.Empty, () => HealthCheckResult.Healthy());
        
        services.AddKafkaHandlers(_configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.UseGrpcServices();
            endpointRouteBuilder.MapGrpcHealthChecksService();
        });
    }
}