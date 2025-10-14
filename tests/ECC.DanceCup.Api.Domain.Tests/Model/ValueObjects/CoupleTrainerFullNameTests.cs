using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class CoupleTrainerFullNameTests
{
    [Theory]
    [InlineData("Trainer Name")]
    [InlineData("John Trainer")]
    [InlineData("A")]
    [InlineData("Very Long Trainer Full Name")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var trainerFullName = CoupleTrainerFullName.From(value);

        // Assert

        trainerFullName.Should().NotBeNull();
        trainerFullName.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Create_FromInvalidValue_ShouldBeNull(string value)
    {
        // Arrange

        // Act

        var trainerFullName = CoupleTrainerFullName.From(value);

        // Assert

        trainerFullName.Should().BeNull();
    }

    [Fact]
    public void Create_FromNullValue_ShouldBeNull()
    {
        // Arrange

        // Act

        var trainerFullName = CoupleTrainerFullName.From(null!);

        // Assert

        trainerFullName.Should().BeNull();
    }
}
