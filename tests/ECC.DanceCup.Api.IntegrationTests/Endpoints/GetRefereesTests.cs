using Dapper;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;
using Npgsql;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class GetRefereesTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;
    private readonly string _postgresConnectionString;

    public GetRefereesTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
        _postgresConnectionString = factory.PostgresConnectionString;
    }

    [Fact]
    public async Task GetReferees_ShouldReturnAllReferees()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "referees" ("version", "created_at", "changed_at", "full_name") values
            (1, now(), now(), 'Referee Test One'),
            (1, now(), now(), 'Referee Test Two'),
            (1, now(), now(), 'Referee Test Three');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetRefereesRequest
        {
            PageNumber = 1,
            PageSize = 100
        };

        // Act

        var response = await danceCupApiClient.GetRefereesAsync(request);

        // Assert

        response.Referees.Should().Contain(r => r.FullName == "Referee Test One");
        response.Referees.Should().Contain(r => r.FullName == "Referee Test Two");
        response.Referees.Should().Contain(r => r.FullName == "Referee Test Three");
    }

    [Fact]
    public async Task GetReferees_WithFullNameFilter_ShouldReturnMatchingReferees()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "referees" ("version", "created_at", "changed_at", "full_name") values
            (1, now(), now(), 'John Smith'),
            (1, now(), now(), 'Jane Doe'),
            (1, now(), now(), 'John Doe');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetRefereesRequest
        {
            FullName = "John Smith",
            PageNumber = 1,
            PageSize = 10
        };

        // Act

        var response = await danceCupApiClient.GetRefereesAsync(request);

        // Assert

        response.Referees.Should().ContainSingle();
        response.Referees.First().FullName.Should().Be("John Smith");
    }

    [Fact]
    public async Task GetReferees_WithPagination_ShouldRespectPageSize()
    {
        // Arrange

        await using var connection = new NpgsqlConnection(_postgresConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            """
            insert into "referees" ("version", "created_at", "changed_at", "full_name") values
            (1, now(), now(), 'PageTest Ref 1'),
            (1, now(), now(), 'PageTest Ref 2'),
            (1, now(), now(), 'PageTest Ref 3'),
            (1, now(), now(), 'PageTest Ref 4'),
            (1, now(), now(), 'PageTest Ref 5');
            """
        );

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);

        var request = new GetRefereesRequest
        {
            PageNumber = 1,
            PageSize = 3
        };

        // Act

        var response = await danceCupApiClient.GetRefereesAsync(request);

        // Assert

        // Verify pagination respects the page size - should return at most 3 items
        response.Referees.Count.Should().BeLessThanOrEqualTo(3);
        
        // Verify we have at least some of our test referees
        var ourRefs = response.Referees.Where(r => r.FullName.StartsWith("PageTest Ref")).ToList();
        ourRefs.Should().HaveCountGreaterThanOrEqualTo(1);
    }
}
