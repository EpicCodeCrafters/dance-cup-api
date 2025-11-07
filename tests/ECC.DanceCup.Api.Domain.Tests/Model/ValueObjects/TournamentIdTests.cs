using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class TournamentIdTests
{
    [Fact]
    public void Empty_ShouldReturnZeroValue()
    {
        // Act
        var id = TournamentId.Empty;

        // Assert
        id.Value.Should().Be(0);
    }

    [Theory, AutoMoqData]
    public void From_WithPositiveValue_ShouldReturnTournamentId(long value)
    {
        // Arrange
        var positiveValue = Math.Abs(value) + 1;

        // Act
        var id = TournamentId.From(positiveValue);

        // Assert
        id.Should().NotBeNull();
        id!.Value.Value.Should().Be(positiveValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void From_WithNonPositiveValue_ShouldReturnNull(long value)
    {
        // Act
        var id = TournamentId.From(value);

        // Assert
        id.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void TournamentId_WithSameValue_ShouldBeEqual(long value)
    {
        // Arrange
        var positiveValue = Math.Abs(value) + 1;
        var id1 = TournamentId.From(positiveValue)!.Value;
        var id2 = TournamentId.From(positiveValue)!.Value;

        // Act & Assert
        id1.Should().Be(id2);
        (id1 == id2).Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public void TournamentId_WithDifferentValue_ShouldNotBeEqual(long value1, long value2)
    {
        // Arrange
        var positiveValue1 = Math.Abs(value1) + 1;
        var positiveValue2 = Math.Abs(value2) + 2;
        var id1 = TournamentId.From(positiveValue1)!.Value;
        var id2 = TournamentId.From(positiveValue2)!.Value;

        // Act & Assert
        id1.Should().NotBe(id2);
        (id1 != id2).Should().BeTrue();
    }
}
