using ECC.DanceCup.Api.Utils;
using Serilog;
using Serilog.Sinks.Kafka;

namespace ECC.DanceCup.Api.Logging;

public static class Registrar
{
    public static IServiceCollection AddCustomLogging(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services
            .AddSerilog(loggerConfiguration => loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", Constants.ServiceName)
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Kafka(
                    bootstrapServers: configuration["KafkaOptions:BootstrapServers"],
                    topic: configuration["KafkaOptions:Topics:DanceCupLogs:Name"]
                )
                .WriteTo.Console()
            );
        
        return services;
    }

    public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseSerilogRequestLogging();
        
        return applicationBuilder;
    }
}