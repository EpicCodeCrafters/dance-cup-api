using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class RefereeFullNameTests
{
    [Theory]
    [InlineData("Иванов Иван")]
    [InlineData("Иванов Иван Иваныч")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var refereeFullName = RefereeFullName.From(value);

        // Assert

        refereeFullName.Should().NotBeNull();
        refereeFullName.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_FromInvalidValue_ShouldBeNull(string value)
    {
        // Arrange

        // Act

        var refereeFullName = RefereeFullName.From(value);

        // Assert

        refereeFullName.Should().BeNull();
    }
}