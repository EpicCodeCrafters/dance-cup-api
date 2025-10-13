using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class UsernameTests
{
    [Theory]
    [InlineData("user")]
    [InlineData("john_doe")]
    [InlineData("test-user-123")]
    [InlineData("A")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var username = Username.From(value);

        // Assert

        username.Should().NotBeNull();
        username.Value.Value.Should().Be(value);
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

        var username = Username.From(value);

        // Assert

        username.Should().BeNull();
    }

    [Fact]
    public void Create_FromNullValue_ShouldBeNull()
    {
        // Arrange

        // Act

        var username = Username.From(null!);

        // Assert

        username.Should().BeNull();
    }
}
