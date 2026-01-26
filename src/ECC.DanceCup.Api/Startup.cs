using ECC.DanceCup.Api.Application;
using ECC.DanceCup.Api.Domain;
using ECC.DanceCup.Api.Infrastructure.Caching;
using ECC.DanceCup.Api.Infrastructure.Metrics;
using ECC.DanceCup.Api.Infrastructure.ObjectStorage;
using ECC.DanceCup.Api.Infrastructure.Storage;
using ECC.DanceCup.Api.Infrastructure.TgApi;
using ECC.DanceCup.Api.Presentation.Grpc;
using ECC.DanceCup.Api.Presentation.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

namespace ECC.DanceCup.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        services.AddApplicationServices();

        services.AddStorage(configuration);
        services.AddCaching(configuration);
        services.AddObjectStorage(configuration);
        services.AddTgApi();

        services.AddGrpcServices();
        services.AddGrpcHealthChecks()
            .AddCheck(string.Empty, () => HealthCheckResult.Healthy())
            .ForwardToPrometheus();

        services.AddKafkaHandlers(configuration);

        services.AddCustomMetrics();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseGrpcMetrics();
        app.UseHttpMetrics();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.UseGrpcServices();
            endpointRouteBuilder.MapGrpcHealthChecksService();
            endpointRouteBuilder.MapMetrics();
        });
    }
}