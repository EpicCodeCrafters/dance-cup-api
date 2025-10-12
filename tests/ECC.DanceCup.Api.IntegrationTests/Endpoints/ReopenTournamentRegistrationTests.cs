using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class ReopenTournamentRegistrationTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public ReopenTournamentRegistrationTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task ReopenTournamentRegistration_ShouldChangeStateBackToRegistrationInProgress()
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 700, 'testuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 700), 'Test Tournament', 'Test Description', now() + interval '30 days', 'RegistrationFinished', now() - interval '10 days', now() - interval '1 day', null, null);
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Test Tournament' and "state" = 'RegistrationFinished';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new ReopenTournamentRegistrationRequest
        {
            TournamentId = tournamentId
        };

        // Act

        await danceCupApiClient.ReopenTournamentRegistrationAsync(request);

        // Assert

        var tournament = await connection.QueryFirstOrDefaultAsync<TournamentTestModel>(
            """
            select * from "tournaments" where "id" = @TournamentId;
            """,
            new { TournamentId = tournamentId }
        );

        tournament.Should().NotBeNull();
        tournament.State.Should().Be("RegistrationInProgress");
        tournament.Version.Should().Be(2);
        tournament.RegistrationFinishedAt.Should().BeNull();
        tournament.ChangedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
    }

    private class TournamentTestModel
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string State { get; set; } = string.Empty;
        public DateTime? RegistrationStartedAt { get; set; }
        public DateTime? RegistrationFinishedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
