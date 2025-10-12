using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class GetTournamentRegistrationResultTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public GetTournamentRegistrationResultTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task GetTournamentRegistrationResult_ShouldReturnCouplesGroupedByCategories()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 800, 'testuser');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 800), 'Test Tournament', 'Test Description', now() + interval '30 days', 'RegistrationInProgress', now(), null, null, null);
            
            insert into "categories" ("tournament_id", "name") values
            ((select "id" from "tournaments" where "name" = 'Test Tournament'), 'Category A'),
            ((select "id" from "tournaments" where "name" = 'Test Tournament'), 'Category B');
            
            insert into "couples" ("tournament_id", "first_participant_full_name", "second_participant_full_name", "dance_organization_name", "first_trainer_full_name", "second_trainer_full_name") values
            ((select "id" from "tournaments" where "name" = 'Test Tournament'), 'John Doe', 'Jane Smith', 'Dance Org 1', 'Trainer A', 'Trainer B'),
            ((select "id" from "tournaments" where "name" = 'Test Tournament'), 'Alice Brown', 'Bob White', 'Dance Org 2', 'Trainer C', null),
            ((select "id" from "tournaments" where "name" = 'Test Tournament'), 'Charlie Green', 'Diana Blue', 'Dance Org 3', null, null);
            
            insert into "categories_couples" ("category_id", "couple_id") values
            ((select "id" from "categories" where "name" = 'Category A' limit 1), (select "id" from "couples" where "first_participant_full_name" = 'John Doe')),
            ((select "id" from "categories" where "name" = 'Category A' limit 1), (select "id" from "couples" where "first_participant_full_name" = 'Alice Brown')),
            ((select "id" from "categories" where "name" = 'Category B' limit 1), (select "id" from "couples" where "first_participant_full_name" = 'Charlie Green'));
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Test Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentRegistrationResultRequest
        {
            TournamentId = tournamentId
        };

        // Act

        var response = await danceCupApiClient.GetTournamentRegistrationResultAsync(request);

        // Assert

        response.Items.Should().HaveCount(3);
        
        var categoryAResults = response.Items.Where(r => r.CategoryName == "Category A").ToList();
        categoryAResults.Should().HaveCount(2);
        categoryAResults.Should().Contain(r => r.FirstParticipantFullName == "John Doe" && r.SecondParticipantFullName == "Jane Smith");
        categoryAResults.Should().Contain(r => r.FirstParticipantFullName == "Alice Brown" && r.SecondParticipantFullName == "Bob White");
        
        var johnDoeResult = categoryAResults.First(r => r.FirstParticipantFullName == "John Doe");
        johnDoeResult.DanceOrganizationName.Should().Be("Dance Org 1");
        johnDoeResult.FirstTrainerFullName.Should().Be("Trainer A");
        johnDoeResult.SecondTrainerFullName.Should().Be("Trainer B");
        
        var categoryBResults = response.Items.Where(r => r.CategoryName == "Category B").ToList();
        categoryBResults.Should().ContainSingle();
        categoryBResults.First().FirstParticipantFullName.Should().Be("Charlie Green");
        categoryBResults.First().SecondParticipantFullName.Should().Be("Diana Blue");
    }

    [Fact]
    public async Task GetTournamentRegistrationResult_WhenNoRegistrations_ShouldReturnEmpty()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 900, 'testuser2');
            
            insert into "tournaments" ("version", "created_at", "changed_at", "user_id", "name", "description", "date", "state", "registration_started_at", "registration_finished_at", "started_at", "finished_at") values
            (1, now(), now(), (select "id" from "users" where "external_id" = 900), 'Empty Tournament', 'Test Description', now() + interval '30 days', 'Created', null, null, null, null);
            """
        );

        var tournamentId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "tournaments" where "name" = 'Empty Tournament';
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetTournamentRegistrationResultRequest
        {
            TournamentId = tournamentId
        };

        // Act

        var response = await danceCupApiClient.GetTournamentRegistrationResultAsync(request);

        // Assert

        response.Items.Should().BeEmpty();
    }
}
