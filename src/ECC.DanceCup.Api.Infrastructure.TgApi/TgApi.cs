using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using ECC.DanceCup.Api.Infrastructure.TgApi.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace ECC.DanceCup.Api.Infrastructure.TgApi;

public class TgApi : ITgApi
{
    private readonly TelegramBotClient _botClient;
    private readonly long _chatId;
    private readonly ILogger<TgApi> _logger;

    public TgApi(IOptions<TgApiOptions> options, ILogger<TgApi> logger)
    {
        _logger = logger;
        var botToken = options.Value.BotToken;
        _chatId = options.Value.ChatId;
        
        if (string.IsNullOrWhiteSpace(botToken))
        {
            _logger.LogWarning("Telegram bot token не настроен");
            _botClient = null!;
            return;
        }
        
        _botClient = new TelegramBotClient(botToken);
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        if (_botClient is null)
        {
            _logger.LogWarning("Telegram bot не настроен, сообщение не отправлено: {Message}", message);
            return;
        }

        try
        {
            await _botClient.SendTextMessageAsync(
                chatId: _chatId,
                text: message,
                cancellationToken: cancellationToken
            );
            
            _logger.LogInformation("Telegram уведомление отправлено: {Message}", message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка отправки Telegram уведомления: {Message}", message);
        }
    }
}