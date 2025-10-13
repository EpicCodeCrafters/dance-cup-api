using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class CoupleParticipantFullNameTests
{
    [Theory]
    [InlineData("John Doe")]
    [InlineData("Alice Smith")]
    [InlineData("A")]
    [InlineData("Very Long Participant Full Name With Multiple Words")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var participantFullName = CoupleParticipantFullName.From(value);

        // Assert

        participantFullName.Should().NotBeNull();
        participantFullName.Value.Value.Should().Be(value);
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

        var participantFullName = CoupleParticipantFullName.From(value);

        // Assert

        participantFullName.Should().BeNull();
    }

    [Fact]
    public void Create_FromNullValue_ShouldBeNull()
    {
        // Arrange

        // Act

        var participantFullName = CoupleParticipantFullName.From(null!);

        // Assert

        participantFullName.Should().BeNull();
    }
}
