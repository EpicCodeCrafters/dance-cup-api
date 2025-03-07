namespace ECC.DanceCup.Api.Presentation.Kafka.Events.Payloads;

public record UserCreatedPayload(
    long UserId,
    string Username
)
{
    public const string EventType = "UserCreated";
}