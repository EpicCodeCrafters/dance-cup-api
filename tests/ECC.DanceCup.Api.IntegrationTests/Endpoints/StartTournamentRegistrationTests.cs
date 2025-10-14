using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class StartTournamentRegistrationTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public StartTournamentRegistrationTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task StartTournamentRegistration_ShouldChangeStateToRegistrationInProgress()
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 500, 'testuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 500), 'Test Tournament', 'Test Description', now() + interval '30 days', 'Created', null, null, null, null);
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Test Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new StartTournamentRegistrationRequest
        {
            TournamentId = tournamentId
        };

        // Act

        await danceCupApiClient.StartTournamentRegistrationAsync(request);

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
        tournament.RegistrationStartedAt.Should().NotBeNull();
        tournament.RegistrationStartedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
        tournament.ChangedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
    }

    [Fact]
    public async Task StartTournamentRegistration_WhenTournamentNotFound_ShouldReturnError()
    {
        // Arrange

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new StartTournamentRegistrationRequest
        {
            TournamentId = 999999
        };

        // Act & Assert

        var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(
            async () => await danceCupApiClient.StartTournamentRegistrationAsync(request)
        );

        exception.StatusCode.Should().Be(Grpc.Core.StatusCode.NotFound);
    }

    [Fact]
    public async Task StartTournamentRegistration_WhenAlreadyInProgress_ShouldReturnError()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 501, 'testuser501');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 501), 'Already In Progress', 'Test', now() + interval '30 days', 'RegistrationInProgress', now(), null, null, null);
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Already In Progress';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new StartTournamentRegistrationRequest
        {
            TournamentId = tournamentId
        };

        // Act & Assert

        var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(
            async () => await danceCupApiClient.StartTournamentRegistrationAsync(request)
        );

        exception.StatusCode.Should().Be(Grpc.Core.StatusCode.InvalidArgument);
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
