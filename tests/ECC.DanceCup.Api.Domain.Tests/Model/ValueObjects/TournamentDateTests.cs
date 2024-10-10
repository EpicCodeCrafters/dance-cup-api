using ECC.DanceCup.Api.Domain.Model;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class ToutnamentDateTests
{
    public void Create_ShouldContainOnlyDate()
    {
        // Arrange

        var dateTime = DateTime.UtcNow;

        // Act

        var tournamentDate = TournamentDate.From(dateTime);

        // Assert

        tournamentDate.Should().NotBeNull();
        tournamentDate.Value.Value.Should().Be(dateTime.Date);
    }
}