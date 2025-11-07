using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Core;

public class AggregateVersionTests
{
    [Fact]
    public void Default_ShouldReturnVersionOne()
    {
        // Act
        var version = AggregateVersion.Default;

        // Assert
        version.Value.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void From_WithPositiveValue_ShouldReturnVersion(int value)
    {
        // Arrange
        var positiveValue = Math.Abs(value) + 1;

        // Act
        var version = AggregateVersion.From(positiveValue);

        // Assert
        version.Should().NotBeNull();
        version!.Value.Value.Should().Be(positiveValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void From_WithNonPositiveValue_ShouldReturnNull(int value)
    {
        // Act
        var version = AggregateVersion.From(value);

        // Assert
        version.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void Increase_ShouldReturnIncrementedVersion(int value)
    {
        // Arrange
        var positiveValue = Math.Abs(value) + 1;
        var version = AggregateVersion.From(positiveValue)!.Value;

        // Act
        var increasedVersion = version.Increase();

        // Assert
        increasedVersion.Value.Should().Be(positiveValue + 1);
    }

    [Fact]
    public void Increase_OnDefaultVersion_ShouldReturnVersionTwo()
    {
        // Arrange
        var version = AggregateVersion.Default;

        // Act
        var increasedVersion = version.Increase();

        // Assert
        increasedVersion.Value.Should().Be(2);
    }

    [Theory, AutoMoqData]
    public void Increase_MultipleTimes_ShouldIncrementCorrectly(int value)
    {
        // Arrange
        var positiveValue = Math.Abs(value) + 1;
        var version = AggregateVersion.From(positiveValue)!.Value;

        // Act
        var version2 = version.Increase();
        var version3 = version2.Increase();
        var version4 = version3.Increase();

        // Assert
        version2.Value.Should().Be(positiveValue + 1);
        version3.Value.Should().Be(positiveValue + 2);
        version4.Value.Should().Be(positiveValue + 3);
    }
}
