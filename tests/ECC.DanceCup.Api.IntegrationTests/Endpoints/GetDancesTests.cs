using System.Diagnostics;
using ECC.DanceCup.Api.Presentation.Grpc;
using FluentAssertions;
using Grpc.Net.Client;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class GetDancesTests : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;

    public GetDancesTests(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDances_ShouldReturnDances()
    {
        // Arrange

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);
        
        // Act

        var watch1 = Stopwatch.StartNew();
        var response1 = await danceCupApiClient.GetDancesAsync(new GetDancesRequest());
        var time1 = watch1.ElapsedMilliseconds;
        
        var watch2 = Stopwatch.StartNew();
        var response2 = await danceCupApiClient.GetDancesAsync(new GetDancesRequest());
        var time2 = watch2.ElapsedMilliseconds;

        // Assert

        var expectedDances = new[]
        {
            new Dance { Id = 1, Name = "Медленный вальс", ShortName = "W" },
            new Dance { Id = 2, Name = "Танго", ShortName = "T" },
            new Dance { Id = 3, Name = "Венский вальс", ShortName = "V" },
            new Dance { Id = 4, Name = "Фокстрот", ShortName = "F" },
            new Dance { Id = 5, Name = "Квикстеп", ShortName = "Q" },
            new Dance { Id = 6, Name = "Самба", ShortName = "S" },
            new Dance { Id = 7, Name = "Ча-ча-ча", ShortName = "Ch" },
            new Dance { Id = 8, Name = "Румба", ShortName = "R" },
            new Dance { Id = 9, Name = "Пасодобль", ShortName = "P" },
            new Dance { Id = 10, Name = "Джайв", ShortName = "J" }
        };

        response1.Dances.Should().BeEquivalentTo(expectedDances);
        response2.Dances.Should().BeEquivalentTo(expectedDances);

        time2.Should().BeLessThan(time1);
    }
}