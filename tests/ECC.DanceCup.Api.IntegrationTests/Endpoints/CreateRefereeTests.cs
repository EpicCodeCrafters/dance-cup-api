using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class CreateRefereeTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;

    private readonly string _postgresConnectionString;

    public CreateRefereeTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();

        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Theory, AutoMoqData]
    public async Task CreateReferee_ShouldCreateReferee(
        string refereeFullName)
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new CreateRefereeRequest
        {
            FullName = refereeFullName
        };

        // Act

        var response = await danceCupApiClient.CreateRefereeAsync(request);

        // Assert

        response.RefereeId.Should().BePositive();

        var referee = await connection.QueryFirstOrDefaultAsync<RefereeTestModel>(
            """
            select * from "referees" where "full_name" = @FullName;
            """,
            new { FullName = refereeFullName }
        );

        referee.Should().NotBeNull();
        referee.Id.Should().BePositive();
        referee.Version.Should().Be(1);
        referee.CreatedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
        referee.ChangedAt.Should().Be(referee.CreatedAt);
        referee.FullName.Should().Be(refereeFullName);
    }

    private class RefereeTestModel
    {
        public long Id { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ChangedAt { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}