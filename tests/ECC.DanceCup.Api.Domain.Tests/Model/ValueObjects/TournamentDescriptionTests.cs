using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class TournamentDescriptionTests
{
    [Theory]
    [InlineData("величайший турнир по чачаче")]
    [InlineData("501 203 @собака")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        //Arrange

        //Act

        var tournamentDescription = TournamentDescription.From(value);

        //Assert

        tournamentDescription.Should().NotBeNull();
        tournamentDescription.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Create_FromValidValue_ShouldBeNull(string value)
    {
        //Arrange

        //Act

        var tournamentDescription = TournamentDescription.From(value);

        //Assert

        tournamentDescription.Should().BeNull();
    }
}