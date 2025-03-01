namespace ECC.DanceCup.Api.Presentation.Kafka.Events;

public record DanceCupEvent(
    string Type,
    string Payload
);