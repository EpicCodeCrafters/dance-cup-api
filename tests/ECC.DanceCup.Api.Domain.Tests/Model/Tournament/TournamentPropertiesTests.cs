using AutoFixture;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class TournamentPropertiesTests
{
    [Theory, AutoMoqData]
    public void CategoriesCount_ShouldReturnCorrectCount(IFixture fixture)
    {
        // Arrange
        var categories = fixture.CreateMany<Category>(3).ToList();
        var tournament = fixture.CreateTournament(categories: categories);

        // Act
        var count = tournament.CategoriesCount;

        // Assert
        count.Should().Be(3);
    }

    [Theory, AutoMoqData]
    public void Categories_ShouldReturnReadOnlyCollection(IFixture fixture)
    {
        // Arrange
        var categories = fixture.CreateMany<Category>(2).ToList();
        var tournament = fixture.CreateTournament(categories: categories);

        // Act
        var result = tournament.Categories;

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeAssignableTo<IReadOnlyCollection<Category>>();
    }

    [Theory, AutoMoqData]
    public void CouplesCount_WhenNoCouples_ShouldReturnZero(IFixture fixture)
    {
        // Arrange
        var tournament = fixture.CreateTournament(couples: new List<Couple>());

        // Act
        var count = tournament.CouplesCount;

        // Assert
        count.Should().Be(0);
    }

    [Theory, AutoMoqData]
    public void Couples_ShouldReturnReadOnlyCollection(IFixture fixture)
    {
        // Arrange
        var couples = fixture.CreateMany<Couple>(3).ToList();
        var tournament = fixture.CreateTournament(couples: couples);

        // Act
        var result = tournament.Couples;

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeAssignableTo<IReadOnlyCollection<Couple>>();
    }

    [Theory, AutoMoqData]
    public void RegisterCouple_ShouldIncrementCouplesCount(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        IFixture fixture)
    {
        // Arrange
        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress,
            couples: new List<Couple>()
        );

        var initialCount = tournament.CouplesCount;
        var categoriesIds = tournament.Categories.Select(c => c.Id).ToArray();

        // Act
        tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            null, null, null, null,
            categoriesIds
        );

        // Assert
        tournament.CouplesCount.Should().Be(initialCount + 1);
    }

    [Theory, AutoMoqData]
    public void Tournament_InitialState_ShouldBeCreated(IFixture fixture)
    {
        // Arrange & Act
        var tournament = fixture.CreateTournament(state: TournamentState.Created);

        // Assert
        tournament.State.Should().Be(TournamentState.Created);
        tournament.RegistrationStartedAt.Should().BeNull();
        tournament.RegistrationFinishedAt.Should().BeNull();
        tournament.StartedAt.Should().BeNull();
        tournament.FinishedAt.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void Tournament_AfterStartingRegistration_ShouldHaveCorrectState(IFixture fixture)
    {
        // Arrange
        var testStartTime = DateTime.UtcNow;
        var tournament = fixture.CreateTournament(state: TournamentState.Created);

        // Act
        tournament.StartRegistration();

        // Assert
        tournament.State.Should().Be(TournamentState.RegistrationInProgress);
        tournament.RegistrationStartedAt.Should().NotBeNull();
        tournament.RegistrationStartedAt.Should().BeAfter(testStartTime);
        tournament.RegistrationFinishedAt.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void Tournament_AfterFinishingRegistration_ShouldHaveCorrectState(IFixture fixture)
    {
        // Arrange
        var testStartTime = DateTime.UtcNow;
        var tournament = fixture.CreateTournament(state: TournamentState.RegistrationInProgress);

        // Act
        tournament.FinishRegistration();

        // Assert
        tournament.State.Should().Be(TournamentState.RegistrationFinished);
        tournament.RegistrationFinishedAt.Should().NotBeNull();
        tournament.RegistrationFinishedAt.Should().BeAfter(testStartTime);
    }

    [Theory, AutoMoqData]
    public void Tournament_AfterReopeningRegistration_ShouldResetFinishedTime(IFixture fixture)
    {
        // Arrange
        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationFinished,
            registrationFinishedAt: DateTime.UtcNow.AddDays(-1)
        );

        // Act
        tournament.ReopenRegistration();

        // Assert
        tournament.State.Should().Be(TournamentState.RegistrationInProgress);
        tournament.RegistrationFinishedAt.Should().BeNull();
    }
}
