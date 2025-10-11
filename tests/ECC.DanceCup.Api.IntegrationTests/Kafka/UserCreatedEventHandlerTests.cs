using System.Text.Json;
using Confluent.Kafka;
using Dapper;
using ECC.DanceCup.Api.Presentation.Kafka.Events;
using ECC.DanceCup.Api.Presentation.Kafka.Events.Payloads;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Kafka;

public class UserCreatedEventHandlerTests : IClassFixture<DanceCupApiFactory>
{
    private readonly string _postgresConnectionString;
    private readonly string _kafkaBootstrapServers;

    public UserCreatedEventHandlerTests(DanceCupApiFactory factory)
    {
        factory.CreateClient();
        
        _postgresConnectionString = factory.PostgresConnectionString;
        _kafkaBootstrapServers = factory.KafkaBoostrapServers;
    }
    
    [Theory, AutoMoqData]
    public async Task EventPublished_ShouldCreateUser(
        long externalId,
        string username)
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;
        
        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaBootstrapServers
        };
        
        using var producer = new ProducerBuilder<string, string>(producerConfig).Build();

        var message = new Message<string, string>
        {
            Key = "22",
            Value = JsonSerializer.Serialize(new DanceCupEvent(
                Type: UserCreatedPayload.EventType,
                Payload: JsonSerializer.Serialize(new UserCreatedPayload(externalId, username))))
        };

        // Act

        producer.Produce("dance_cup_events", message);
        producer.Flush(TimeSpan.FromSeconds(5));

        await Task.Delay(TimeSpan.FromSeconds(5));

        // Assert

        var user = await connection.QueryFirstOrDefaultAsync<UserTestModel>(
            """
            select * from "users" where "external_id" = @externalId;
            """,
            new { ExternalId = externalId }
        );

        user.Should().NotBeNull();
        user.Id.Should().BePositive();
        user.Version.Should().Be(1);
        user.CreatedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
        user.ChangedAt.Should().Be(user.CreatedAt);
        user.ExternalId.Should().Be(externalId);
        user.Username.Should().Be(username);
    }
    
    private class UserTestModel
    {
        public long Id { get; set; }
        
        public int Version { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime ChangedAt { get; set; }
        
        public long ExternalId { get; set; }
        
        public string Username { get; set; } = string.Empty;
    }
}