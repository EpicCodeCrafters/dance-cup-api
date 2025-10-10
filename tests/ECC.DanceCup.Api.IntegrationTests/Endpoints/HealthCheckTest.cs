using FluentAssertions;
using Grpc.Health.V1;
using Grpc.Net.Client;

namespace ECC.DanceCup.Api.IntegrationTests.Endpoints;

public class HealthCheckTest : IClassFixture<DanceCupApiFactory>
{
    private readonly HttpClient _client;

    public HealthCheckTest(DanceCupApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthy()
    {
        // Arrange

        var channel = GrpcChannel.ForAddress(_client.BaseAddress!, new GrpcChannelOptions { HttpClient = _client });
        var healthClient = new Health.HealthClient(channel);
        
        // Act

        var response = await healthClient.CheckAsync(new HealthCheckRequest());

        // Assert

        response.Status.Should().Be(HealthCheckResponse.Types.ServingStatus.Serving);
    }
}