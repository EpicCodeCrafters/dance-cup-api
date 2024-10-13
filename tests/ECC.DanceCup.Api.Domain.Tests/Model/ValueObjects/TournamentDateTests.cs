using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class TournamentDateTests
{
    [Fact]
    public void Create_ShouldContainOnlyDate()
    {
        // Arrange

        var value = DateTime.UtcNow;

        // Act

        var tournamentDate = TournamentDate.From(value);

        // Assert

        tournamentDate.Should().NotBeNull();
        tournamentDate.Value.Value.Should().Be(value.Date);
    }
}