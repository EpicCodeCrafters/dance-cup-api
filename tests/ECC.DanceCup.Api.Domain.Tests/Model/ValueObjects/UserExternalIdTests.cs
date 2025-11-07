using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class UserExternalIdTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Create_FromValidValue_ShouldNotBeNull(long value)
    {
        // Arrange

        // Act

        var userExternalId = UserExternalId.From(value);

        // Assert

        userExternalId.Should().NotBeNull();
        userExternalId.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(int.MinValue)]
    [InlineData(long.MinValue)]
    public void Create_FromInvalidValue_ShouldBeNull(long value)
    {
        // Arrange

        // Act

        var userExternalId = UserExternalId.From(value);

        // Assert

        userExternalId.Should().BeNull();
    }
}
