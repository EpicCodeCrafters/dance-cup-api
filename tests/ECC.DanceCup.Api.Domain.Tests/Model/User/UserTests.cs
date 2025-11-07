using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.User;

public class UserTests
{
    [Theory, AutoMoqData]
    public void User_ShouldHaveCorrectProperties(
        UserId id,
        AggregateVersion version,
        DateTime createdAt,
        DateTime changedAt,
        UserExternalId externalId,
        Username username)
    {
        // Arrange & Act
        var user = new ECC.DanceCup.Api.Domain.Model.UserAggregate.User(
            id,
            version,
            createdAt,
            changedAt,
            externalId,
            username
        );

        // Assert
        user.Id.Should().Be(id);
        user.Version.Should().Be(version);
        user.CreatedAt.Should().Be(createdAt);
        user.ChangedAt.Should().Be(changedAt);
        user.ExternalId.Should().Be(externalId);
        user.Username.Should().Be(username);
    }

    [Theory, AutoMoqData]
    public void User_CreatedAndChangedAt_ShouldBeEqual(
        UserId id,
        UserExternalId externalId,
        Username username)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var user = new ECC.DanceCup.Api.Domain.Model.UserAggregate.User(
            id,
            AggregateVersion.Default,
            now,
            now,
            externalId,
            username
        );

        // Assert
        user.CreatedAt.Should().Be(user.ChangedAt);
    }

    [Theory, AutoMoqData]
    public void User_WithDefaultVersion_ShouldHaveVersion1(
        UserId id,
        UserExternalId externalId,
        Username username)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var user = new ECC.DanceCup.Api.Domain.Model.UserAggregate.User(
            id,
            AggregateVersion.Default,
            now,
            now,
            externalId,
            username
        );

        // Assert
        user.Version.Value.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void User_WithEmptyId_ShouldHaveEmptyId(
        UserExternalId externalId,
        Username username)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var user = new ECC.DanceCup.Api.Domain.Model.UserAggregate.User(
            UserId.Empty,
            AggregateVersion.Default,
            now,
            now,
            externalId,
            username
        );

        // Assert
        user.Id.Should().Be(UserId.Empty);
        user.Id.Value.Should().Be(0);
    }

    [Theory, AutoMoqData]
    public void User_ExternalIdAndUsername_ShouldBeImmutable(
        UserId id,
        UserExternalId externalId,
        Username username)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var user = new ECC.DanceCup.Api.Domain.Model.UserAggregate.User(
            id,
            AggregateVersion.Default,
            now,
            now,
            externalId,
            username
        );

        var originalExternalId = user.ExternalId;
        var originalUsername = user.Username;

        // Act - properties should not be changeable

        // Assert
        user.ExternalId.Should().Be(originalExternalId);
        user.Username.Should().Be(originalUsername);
    }
}
