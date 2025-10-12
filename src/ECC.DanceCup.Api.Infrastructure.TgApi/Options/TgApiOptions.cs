namespace ECC.DanceCup.Api.Infrastructure.TgApi.Options;

public class TgApiOptions
{
    public string BotToken { get; set; } = string.Empty;
    
    public long ChatId { get; set; }
}
