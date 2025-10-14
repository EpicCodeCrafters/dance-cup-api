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
    public async Task RegisterCoupleForTournament_ShouldCreateCoupleInTournamentWithCategoryLink()
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

    [Fact]
    public async Task RegisterCoupleForTournament_WithMultipleCategories_ShouldLinkToAllCategories()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "tournaments" values (2, 1, now(), now(), 1, 'T2', 'D2', now(), 'RegistrationInProgress', now(), null, null, null);
            insert into "categories" values (21, 2, 'C21');
            insert into "categories" values (22, 2, 'C22');
            insert into "categories" values (23, 2, 'C23');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new RegisterCoupleForTournamentRequest
        {
            TournamentId = 2,
            CategoriesIds = { 21, 22, 23 },
            DanceOrganizationName = "Multi Cat Org",
            FirstParticipantFullName = "Alice Multi",
            SecondParticipantFullName = "Bob Multi",
            FirstTrainerFullName = "Trainer A",
            SecondTrainerFullName = "Trainer B"
        };

        // Act

        await danceCupApiClient.RegisterCoupleForTournamentAsync(request);

        // Assert

        var coupleCategories = await connection.QueryAsync<CoupleCategoryTestModel>(
            """
            select cc.*, c."name" as "category_name"
            from "categories_couples" cc
            join "categories" c on c."id" = cc."category_id"
            join "couples" cpl on cpl."id" = cc."couple_id"
            where cpl."first_participant_full_name" = 'Alice Multi'
            order by cc."category_id";
            """
        );

        coupleCategories.Should().HaveCount(3);
        coupleCategories.Select(cc => cc.CategoryId).Should().BeEquivalentTo(new[] { 21L, 22L, 23L });
    }

    [Fact]
    public async Task RegisterCoupleForTournament_WithoutSecondParticipant_ShouldCreateSoloCouple()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "tournaments" values (3, 1, now(), now(), 1, 'T3', 'D3', now(), 'RegistrationInProgress', now(), null, null, null);
            insert into "categories" values (31, 3, 'C31');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new RegisterCoupleForTournamentRequest
        {
            TournamentId = 3,
            CategoriesIds = { 31 },
            FirstParticipantFullName = "Solo Dancer"
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
            where cpl."first_participant_full_name" = 'Solo Dancer';
            """
        );

        couple.Should().NotBeNull();
        couple.FirstParticipantFullName.Should().Be("Solo Dancer");
        couple.SecondParticipantFullName.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task RegisterCoupleForTournament_WhenTournamentNotInRegistrationState_ShouldReturnError()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "tournaments" values (4, 1, now(), now(), 1, 'T4', 'D4', now(), 'Created', null, null, null, null);
            insert into "categories" values (41, 4, 'C41');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new RegisterCoupleForTournamentRequest
        {
            TournamentId = 4,
            CategoriesIds = { 41 },
            FirstParticipantFullName = "Wrong State Dancer"
        };

        // Act & Assert

        var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(
            async () => await danceCupApiClient.RegisterCoupleForTournamentAsync(request)
        );

        exception.StatusCode.Should().Be(Grpc.Core.StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task RegisterCoupleForTournament_WhenTournamentNotFound_ShouldReturnError()
    {
        // Arrange

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new RegisterCoupleForTournamentRequest
        {
            TournamentId = 999999,
            CategoriesIds = { 1 },
            FirstParticipantFullName = "Nonexistent Tournament Dancer"
        };

        // Act & Assert

        var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(
            async () => await danceCupApiClient.RegisterCoupleForTournamentAsync(request)
        );

        exception.StatusCode.Should().Be(Grpc.Core.StatusCode.NotFound);
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

    private class CoupleCategoryTestModel
    {
        public long CoupleId { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}