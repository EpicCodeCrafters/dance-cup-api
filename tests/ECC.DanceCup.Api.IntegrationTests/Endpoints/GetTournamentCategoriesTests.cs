using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class GetTournamentCategoriesTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public GetTournamentCategoriesTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task GetTournamentCategories_ShouldReturnAllCategoriesForTournament()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 1001, 'categorytestuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 1001), 'Category Test Tournament', 'Test Description', now() + interval '30 days', 'RegistrationInProgress', now(), null, null, null);
            
            insert into "categories" ("tournament_id", "name") values
            ((select "id" from "tournaments" where "name" = 'Category Test Tournament'), 'Beginners'),
            ((select "id" from "tournaments" where "name" = 'Category Test Tournament'), 'Intermediate'),
            ((select "id" from "tournaments" where "name" = 'Category Test Tournament'), 'Advanced');
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Category Test Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentCategoriesRequest
        {
            TournamentId = tournamentId
        };

        // Act

        var response = await danceCupApiClient.GetTournamentCategoriesAsync(request);

        // Assert

        response.Categories.Should().HaveCount(3);
        response.Categories.Should().Contain(c => c.Name == "Beginners");
        response.Categories.Should().Contain(c => c.Name == "Intermediate");
        response.Categories.Should().Contain(c => c.Name == "Advanced");
        
        // Verify all categories have IDs
        response.Categories.All(c => c.Id > 0).Should().BeTrue();
    }

    [Fact]
    public async Task GetTournamentCategories_WhenTournamentHasNoCategories_ShouldReturnEmpty()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 1002, 'nocategorytestuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 1002), 'No Category Tournament', 'Test Description', now() + interval '30 days', 'Created', null, null, null, null);
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'No Category Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentCategoriesRequest
        {
            TournamentId = tournamentId
        };

        // Act

        var response = await danceCupApiClient.GetTournamentCategoriesAsync(request);

        // Assert

        response.Categories.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTournamentCategories_WhenTournamentNotFound_ShouldReturnEmpty()
    {
        // Arrange

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentCategoriesRequest
        {
            TournamentId = 999999
        };

        // Act

        var response = await danceCupApiClient.GetTournamentCategoriesAsync(request);

        // Assert

        response.Categories.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTournamentCategories_ShouldReturnCategoriesInCorrectOrder()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 1003, 'ordertestuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 1003), 'Order Test Tournament', 'Test Description', now() + interval '30 days', 'RegistrationInProgress', now(), null, null, null);
            
            insert into "categories" ("tournament_id", "name") values
            ((select "id" from "tournaments" where "name" = 'Order Test Tournament'), 'Category C'),
            ((select "id" from "tournaments" where "name" = 'Order Test Tournament'), 'Category A'),
            ((select "id" from "tournaments" where "name" = 'Order Test Tournament'), 'Category B');
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Order Test Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentCategoriesRequest
        {
            TournamentId = tournamentId
        };

        // Act

        var response = await danceCupApiClient.GetTournamentCategoriesAsync(request);

        // Assert

        response.Categories.Should().HaveCount(3);
        
        // Verify categories are returned in ID order (insertion order)
        var categories = response.Categories.ToList();
        categories[0].Name.Should().Be("Category C");
        categories[1].Name.Should().Be("Category A");
        categories[2].Name.Should().Be("Category B");
        
        // Verify IDs are in ascending order
        categories[0].Id.Should().BeLessThan(categories[1].Id);
        categories[1].Id.Should().BeLessThan(categories[2].Id);
    }
}
