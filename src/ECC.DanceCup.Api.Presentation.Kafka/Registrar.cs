using ECC.DanceCup.Api.Presentation.Kafka.Consumers;
using ECC.DanceCup.Api.Presentation.Kafka.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Presentation.Kafka;

public static class Registrar
{
    public static IServiceCollection AddKafkaHandlers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHostedService<DanceCupEventsConsumer>();
        
        services.Configure<KafkaOptions>(configuration.GetSection("KafkaOptions"));
        
        return services;
    }
}