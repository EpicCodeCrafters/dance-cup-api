using DanceCup.Api.Domain.Services;
using DanceCup.Api.Tests.Common;
using FluentAssertions;

namespace DanceCup.Api.Domain.Tests.Services;

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

        result.IsSuccess.Should().BeFalse();
    }
}