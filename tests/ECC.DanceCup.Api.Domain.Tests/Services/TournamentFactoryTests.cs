using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class TournamentFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        TournamentFactory tournamentFactory)
    {
        // Arrange
        
        // Act

        var result = tournamentFactory.Create();

        // Assert

        result.IsSuccess.Should().BeTrue();
    }
}