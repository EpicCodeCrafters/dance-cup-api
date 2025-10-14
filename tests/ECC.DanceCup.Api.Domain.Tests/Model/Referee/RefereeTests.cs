using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Referee;

public class RefereeTests
{
    [Theory, AutoMoqData]
    public void Referee_ShouldHaveCorrectProperties(
        RefereeId id,
        AggregateVersion version,
        DateTime createdAt,
        DateTime changedAt,
        RefereeFullName fullName)
    {
        // Arrange & Act
        var referee = new ECC.DanceCup.Api.Domain.Model.RefereeAggregate.Referee(
            id,
            version,
            createdAt,
            changedAt,
            fullName
        );

        // Assert
        referee.Id.Should().Be(id);
        referee.Version.Should().Be(version);
        referee.CreatedAt.Should().Be(createdAt);
        referee.ChangedAt.Should().Be(changedAt);
        referee.FullName.Should().Be(fullName);
    }

    [Theory, AutoMoqData]
    public void Referee_CreatedAndChangedAt_ShouldBeEqual(
        RefereeId id,
        RefereeFullName fullName)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var referee = new ECC.DanceCup.Api.Domain.Model.RefereeAggregate.Referee(
            id,
            AggregateVersion.Default,
            now,
            now,
            fullName
        );

        // Assert
        referee.CreatedAt.Should().Be(referee.ChangedAt);
    }

    [Theory, AutoMoqData]
    public void Referee_WithDefaultVersion_ShouldHaveVersion1(
        RefereeId id,
        RefereeFullName fullName)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var referee = new ECC.DanceCup.Api.Domain.Model.RefereeAggregate.Referee(
            id,
            AggregateVersion.Default,
            now,
            now,
            fullName
        );

        // Assert
        referee.Version.Value.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void Referee_WithEmptyId_ShouldHaveEmptyId(
        RefereeFullName fullName)
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        var referee = new ECC.DanceCup.Api.Domain.Model.RefereeAggregate.Referee(
            RefereeId.Empty,
            AggregateVersion.Default,
            now,
            now,
            fullName
        );

        // Assert
        referee.Id.Should().Be(RefereeId.Empty);
        referee.Id.Value.Should().Be(0);
    }
}
