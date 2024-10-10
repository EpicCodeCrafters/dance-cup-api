using ECC.DanceCup.Api.Domain.Model;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class RefereeIdTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Create_FromIncorrectValue_ShouldNotBeNull(long value)
    {
        // Arrange

        // Act

        var refereeid = RefereeId.From(value);

        // Assert

        refereeid.Should().NotBeNull();
        refereeid.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(long.MinValue)]
    public void Create_FromIncorrectValue_ShouldBeNull(long value)
    {
        // Arrange

        // Act

        var refereeid = RefereeId.From(value);

        // Assert

        refereeid.Should().BeNull();
    }
}