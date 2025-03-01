using System.Text.Json;
using Confluent.Kafka;
using ECC.DanceCup.Api.Application.UseCases.CreateUser;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Presentation.Kafka.Events;
using ECC.DanceCup.Api.Presentation.Kafka.Events.Payloads;
using ECC.DanceCup.Api.Presentation.Kafka.Options;
using ECC.DanceCup.Api.Utils.Extensions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Presentation.Kafka.Consumers;

public class DanceCupEventsConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<KafkaOptions> _options;
    private readonly ILogger<DanceCupEventsConsumer> _logger;

    public DanceCupEventsConsumer(
        IServiceProvider serviceProvider,
        IOptions<KafkaOptions> options, 
        ILogger<DanceCupEventsConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _options.Value.BootstrapServers,
            GroupId = _options.Value.Topics.DanceCupEvents.ConsumerGroup,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(_options.Value.Topics.DanceCupEvents.Name);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var danceCupEvent = JsonSerializer.Deserialize<DanceCupEvent>(consumeResult.Message.Value);
                if (danceCupEvent is null)
                {
                    _logger.LogError("Некорректное событие DanceCupEvent");
                    continue;
                }
                    
                var handleEventResult = await HandleDanceCupEventAsync(danceCupEvent, stoppingToken);
                if (handleEventResult.IsFailed)
                {
                    _logger.LogError("Ошибка обработки события DanceCupEvent: {Reason}", handleEventResult.StringifyErrors());
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException consumeException)
            {
                _logger.LogError("Ошибка чтения события DanceCupEvent: {Reason}", consumeException.Error.Reason);

                if (consumeException.Error.IsFatal)
                {
                    break;
                }
            }
            catch (Exception)
            {
                break;
            }
        }
    }

    private async Task<Result> HandleDanceCupEventAsync(DanceCupEvent danceCupEvent, CancellationToken stoppingToken)
    {
        return danceCupEvent.Type switch
        {
            UserCreatedPayload.EventType => await HandleUserCreated(danceCupEvent, stoppingToken),
            _ => Result.Fail("Неизвестный тип события")
        };
    }

    private async Task<Result> HandleUserCreated(DanceCupEvent danceCupEvent, CancellationToken stoppingToken)
    {
        var payload = JsonSerializer.Deserialize<UserCreatedPayload>(danceCupEvent.Payload);
        if (payload is null)
        {
            return Result.Fail("Некорректное событие UserCreated");
        }
                
        var externalId = UserExternalId.From(payload.UserId);
        if (externalId is null)
        {
            return Result.Fail("Некорректный идентификатор пользователя в событии UserCreated");
        }
                
        var username = Username.From(payload.Username);
        if (username is null)
        {
            return Result.Fail("Некорректное имя пользователя в событии UserCreated");
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        var createUserCommand = new CreateUserUseCase.Command(externalId.Value, username.Value);
        var createUserResult = await sender.Send(createUserCommand, stoppingToken);

        return createUserResult.ToResult();
    }
}