using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class GetTournamentsTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public GetTournamentsTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task GetTournaments_ShouldReturnUserTournaments()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 100, 'user1'),
            (1, now(), now(), 200, 'user2');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 100), 'Tournament 1', 'Description 1', now() + interval '10 days', 'Created', null, null, null, null),
            (1, now(), now(), (select "id" from "users" where "external_id" = 100), 'Tournament 2', 'Description 2', now() + interval '20 days', 'RegistrationInProgress', now(), null, null, null),
            (1, now(), now(), (select "id" from "users" where "external_id" = 200), 'Tournament 3', 'Description 3', now() + interval '30 days', 'Created', null, null, null, null);
            """
        );

        var userId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "users" where "external_id" = 100;
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentsRequest
        {
            UserId = userId,
            PageNumber = 1,
            PageSize = 10
        };

        // Act

        var response = await danceCupApiClient.GetTournamentsAsync(request);

        // Assert

        response.Tournaments.Should().HaveCount(2);
        response.Tournaments.Should().Contain(t => t.Name == "Tournament 1");
        response.Tournaments.Should().Contain(t => t.Name == "Tournament 2");
        response.Tournaments.Should().NotContain(t => t.Name == "Tournament 3");
        
        var tournament1 = response.Tournaments.First(t => t.Name == "Tournament 1");
        tournament1.Description.Should().Be("Description 1");
        tournament1.State.Should().Be("Created");
        
        var tournament2 = response.Tournaments.First(t => t.Name == "Tournament 2");
        tournament2.State.Should().Be("RegistrationInProgress");
    }

    [Fact]
    public async Task GetTournaments_WithPagination_ShouldReturnPagedResults()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 300, 'user3');
            """
        );

        var userId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "users" where "external_id" = 300;
            """
        );

        await connection.ExecuteAsync(
            """
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), @UserId, 'Tournament A', 'Desc A', now() + interval '10 days', 'Created', null, null, null, null),
            (1, now(), now(), @UserId, 'Tournament B', 'Desc B', now() + interval '20 days', 'Created', null, null, null, null),
            (1, now(), now(), @UserId, 'Tournament C', 'Desc C', now() + interval '30 days', 'Created', null, null, null, null),
            (1, now(), now(), @UserId, 'Tournament D', 'Desc D', now() + interval '40 days', 'Created', null, null, null, null),
            (1, now(), now(), @UserId, 'Tournament E', 'Desc E', now() + interval '50 days', 'Created', null, null, null, null);
            """,
            new { UserId = userId }
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentsRequest
        {
            UserId = userId,
            PageNumber = 2,
            PageSize = 2
        };

        // Act

        var response = await danceCupApiClient.GetTournamentsAsync(request);

        // Assert

        response.Tournaments.Should().HaveCount(2);
    }
}
