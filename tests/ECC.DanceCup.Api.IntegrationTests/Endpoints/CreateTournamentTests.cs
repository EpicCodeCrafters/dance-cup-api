using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class CreateTournamentTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public CreateTournamentTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task CreateTournament_ShouldCreateTournamentWithCategoriesAndRelations()
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username") values
            (1, now(), now(), 123, 'testuser');
            insert into "referees" ("version", "created_at", "changed_at", "full_name") values
            (1, now(), now(), 'Referee One'),
            (1, now(), now(), 'Referee Two');
            """
        );

        var userId = await connection.QuerySingleAsync<long>(
            """
            select "id" from "users" where "external_id" = 123;
            """
        );

        var refereeIds = await connection.QueryAsync<long>(
            """
            select "id" from "referees" order by "id";
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var tournamentDate = DateTime.UtcNow.AddDays(30);
        var request = new CreateTournamentRequest
        {
            UserId = userId,
            Name = "Test Tournament",
            Description = "Test Description",
            Date = Timestamp.FromDateTime(tournamentDate),
            CreateCategoryModels =
            {
                new CreateCategoryModel
                {
                    Name = "Category A",
                    DancesIds = { 1 },
                    RefereesIds = { refereeIds.First() }
                },
                new CreateCategoryModel
                {
                    Name = "Category B",
                    DancesIds = { 2 },
                    RefereesIds = { refereeIds.Last() }
                }
            }
        };

        // Act

        var response = await danceCupApiClient.CreateTournamentAsync(request);

        // Assert

        response.TournamentId.Should().BePositive();

        var tournament = await connection.QueryFirstOrDefaultAsync<TournamentTestModel>(
            """
            select * from "tournaments" where "id" = @TournamentId;
            """,
            new { TournamentId = response.TournamentId }
        );

        tournament.Should().NotBeNull();
        tournament.Id.Should().Be(response.TournamentId);
        tournament.Version.Should().Be(1);
        tournament.CreatedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
        tournament.ChangedAt.Should().Be(tournament.CreatedAt);
        tournament.UserId.Should().Be(userId);
        tournament.Name.Should().Be("Test Tournament");
        tournament.Description.Should().Be("Test Description");
        tournament.State.Should().Be("Created");

        var categories = await connection.QueryAsync<CategoryTestModel>(
            """
            select * from "categories" where "tournament_id" = @TournamentId order by "name";
            """,
            new { TournamentId = response.TournamentId }
        );

        categories.Should().HaveCount(2);
        categories.First().Name.Should().Be("Category A");
        categories.Last().Name.Should().Be("Category B");

        var categoryADances = await connection.QueryAsync<long>(
            """
            select cd."dance_id" from "categories_dances" cd
            join "categories" c on c."id" = cd."category_id"
            where c."name" = 'Category A' and c."tournament_id" = @TournamentId
            order by cd."dance_id";
            """,
            new { TournamentId = response.TournamentId }
        );

        categoryADances.Should().ContainSingle().Which.Should().Be(1L);

        var categoryAReferees = await connection.QueryAsync<long>(
            """
            select cr."referee_id" from "categories_referees" cr
            join "categories" c on c."id" = cr."category_id"
            where c."name" = 'Category A' and c."tournament_id" = @TournamentId
            order by cr."referee_id";
            """,
            new { TournamentId = response.TournamentId }
        );

        categoryAReferees.Should().ContainSingle();
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
    }

    private class CategoryTestModel
    {
        public long Id { get; set; }
        public long TournamentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
