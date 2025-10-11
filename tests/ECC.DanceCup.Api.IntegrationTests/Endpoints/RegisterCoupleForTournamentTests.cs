using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class RegisterCoupleForTournamentTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;

    private readonly string _postgresConnectionString;
    
    public RegisterCoupleForTournamentTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();

        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task RegisterCoupleForTournament_ShouldCreateCoupleInTournamentWIthCategoryLink()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "tournaments" values (1, 1, now(), now(), 1, 'T1', 'D', now(), 'RegistrationInProgress', now(), null, null, null);
            insert into "categories" values (11, 1, 'C1');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new RegisterCoupleForTournamentRequest
        {
            TournamentId = 1,
            CategoriesIds = { 11 },
            DanceOrganizationName = "DO",
            FirstParticipantFullName = "Ab Cd Ef",
            SecondParticipantFullName = "Ab Cd Ef2",
            FirstTrainerFullName = "Tr1",
            SecondTrainerFullName = "Tr2"
        };
        
        // Act
        
        await danceCupApiClient.RegisterCoupleForTournamentAsync(request);
        
        // Assert

        var couple = await connection.QueryFirstOrDefaultAsync<CoupleTestModel>(
            """
            select cpl.*
                 , ctg."id" as "category_id"
                 , ctg."name" as "category_name"
            from "couples" as cpl
            join "categories_couples" as cc on cc."couple_id" = cpl."id"
            join "categories" as ctg on ctg."id" = cc."category_id"
            where cpl."first_participant_full_name" = 'Ab Cd Ef';
            """
        );

        couple.Should().NotBeNull();
        couple.Id.Should().BePositive();
        couple.TournamentId.Should().Be(1);
        couple.FirstParticipantFullName.Should().Be("Ab Cd Ef");
        couple.SecondParticipantFullName.Should().Be("Ab Cd Ef2");
        couple.DanceOrganizationName.Should().Be("DO");
        couple.FirstTrainerFullName.Should().Be("Tr1");
        couple.SecondTrainerFullName.Should().Be("Tr2");
        couple.CategoryId.Should().Be(11);
        couple.CategoryName.Should().Be("C1");
    }
    
    private class CoupleTestModel
    {
        public long Id { get; set; }
        
        public long TournamentId { get; set; }
        
        public string FirstParticipantFullName { get; set; } = string.Empty;
        
        public string? SecondParticipantFullName { get; set; }
        
        public string? DanceOrganizationName { get; set; }
        
        public string? FirstTrainerFullName { get; set; }
        
        public string? SecondTrainerFullName { get; set; }
        
        public long CategoryId { get; set; }
        
        public string CategoryName { get; set; } = string.Empty;
    }
}