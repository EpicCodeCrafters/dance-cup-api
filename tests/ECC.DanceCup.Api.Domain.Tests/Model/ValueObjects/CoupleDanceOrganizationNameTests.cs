using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;

public class CoupleDanceOrganizationNameTests
{
    [Theory]
    [InlineData("Dance Studio")]
    [InlineData("Elite Dance Academy")]
    [InlineData("A")]
    [InlineData("Organization Name")]
    public void Create_FromValidValue_ShouldNotBeNull(string value)
    {
        // Arrange

        // Act

        var organizationName = CoupleDanceOrganizationName.From(value);

        // Assert

        organizationName.Should().NotBeNull();
        organizationName.Value.Value.Should().Be(value);
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

        var organizationName = CoupleDanceOrganizationName.From(value);

        // Assert

        organizationName.Should().BeNull();
    }

    [Fact]
    public void Create_FromNullValue_ShouldBeNull()
    {
        // Arrange

        // Act

        var organizationName = CoupleDanceOrganizationName.From(null!);

        // Assert

        organizationName.Should().BeNull();
    }
}
