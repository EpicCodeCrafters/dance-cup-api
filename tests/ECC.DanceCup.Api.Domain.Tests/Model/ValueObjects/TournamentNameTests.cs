using ECC.DanceCup.Api.Domain.Model;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class TournamentNameTests
{
    [Theory]
    [InlineData("закрытая чашка итмо по чачача")]
    [InlineData("Abama is matter")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var tournamentName = TournamentName.From(value);

        // Assert

        tournamentName.Should().NotBeNull();
        tournamentName.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("!@#$%^&*()_+")]
    [InlineData("\t\n\r")]
    [InlineData("")]
    public void Create_FromInvalidValue_ShouldBeNull(string value)
    {
        // Arrange

        // Act

        var tournamentName = TournamentName.From(value);

        // Assert

        tournamentName.Should().BeNull();
    }
}