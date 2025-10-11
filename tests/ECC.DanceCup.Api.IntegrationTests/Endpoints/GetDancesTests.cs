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

        const int otherRequestsCount = 100;

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var danceCupApiClient = new DanceCupApi.DanceCupApiClient(channel);
        
        // Act

        var firstWatch = Stopwatch.StartNew();
        var firstResponse = await danceCupApiClient.GetDancesAsync(new GetDancesRequest());
        var firstTime = firstWatch.ElapsedMilliseconds;

        var otherResponses = new List<GetDancesResponse>();
        var otherTimes = new List<long>();
        foreach (var _ in Enumerable.Range(0, otherRequestsCount))
        {
            var watch = Stopwatch.StartNew();
            var response = await danceCupApiClient.GetDancesAsync(new GetDancesRequest());
            var time = watch.ElapsedMilliseconds;
                
            otherResponses.Add(response);
            otherTimes.Add(time);
        }

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

        firstResponse.Dances.Should().BeEquivalentTo(expectedDances);
        otherResponses.Should().AllSatisfy(
            otherResponse => otherResponse.Dances.Should().BeEquivalentTo(expectedDances)
        );

        otherTimes.Should().AllSatisfy(
            otherTime => otherTime.Should().BeLessThan(firstTime)
        );
    }
}