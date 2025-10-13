using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class CoupleTests
{
    [Theory, AutoMoqData]
    public void Couple_WithAllFields_ShouldHaveCorrectProperties(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName secondParticipantFullName,
        CoupleDanceOrganizationName danceOrganizationName,
        CoupleTrainerFullName firstTrainerFullName,
        CoupleTrainerFullName secondTrainerFullName)
    {
        // Arrange & Act
        var couple = new Couple(
            id,
            tournamentId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName
        );

        // Assert
        couple.Id.Should().Be(id);
        couple.TournamentId.Should().Be(tournamentId);
        couple.FirstParticipantFullName.Should().Be(firstParticipantFullName);
        couple.SecondParticipantFullName.Should().Be(secondParticipantFullName);
        couple.DanceOrganizationName.Should().Be(danceOrganizationName);
        couple.FirstTrainerFullName.Should().Be(firstTrainerFullName);
        couple.SecondTrainerFullName.Should().Be(secondTrainerFullName);
    }

    [Theory, AutoMoqData]
    public void Couple_WithOnlyRequiredFields_ShouldHaveCorrectProperties(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName)
    {
        // Arrange & Act
        var couple = new Couple(
            id,
            tournamentId,
            firstParticipantFullName,
            secondParticipantFullName: null,
            danceOrganizationName: null,
            firstTrainerFullName: null,
            secondTrainerFullName: null
        );

        // Assert
        couple.Id.Should().Be(id);
        couple.TournamentId.Should().Be(tournamentId);
        couple.FirstParticipantFullName.Should().Be(firstParticipantFullName);
        couple.SecondParticipantFullName.Should().BeNull();
        couple.DanceOrganizationName.Should().BeNull();
        couple.FirstTrainerFullName.Should().BeNull();
        couple.SecondTrainerFullName.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void Couple_WithSoloParticipant_ShouldBeValid(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleDanceOrganizationName danceOrganizationName,
        CoupleTrainerFullName firstTrainerFullName)
    {
        // Arrange & Act
        var couple = new Couple(
            id,
            tournamentId,
            firstParticipantFullName,
            secondParticipantFullName: null,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName: null
        );

        // Assert
        couple.FirstParticipantFullName.Should().Be(firstParticipantFullName);
        couple.SecondParticipantFullName.Should().BeNull();
        couple.DanceOrganizationName.Should().Be(danceOrganizationName);
        couple.FirstTrainerFullName.Should().Be(firstTrainerFullName);
        couple.SecondTrainerFullName.Should().BeNull();
    }

    [Theory, AutoMoqData]
    public void Couple_WithoutOrganization_ShouldBeValid(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName secondParticipantFullName,
        CoupleTrainerFullName firstTrainerFullName,
        CoupleTrainerFullName secondTrainerFullName)
    {
        // Arrange & Act
        var couple = new Couple(
            id,
            tournamentId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName: null,
            firstTrainerFullName,
            secondTrainerFullName
        );

        // Assert
        couple.FirstParticipantFullName.Should().Be(firstParticipantFullName);
        couple.SecondParticipantFullName.Should().Be(secondParticipantFullName);
        couple.DanceOrganizationName.Should().BeNull();
        couple.FirstTrainerFullName.Should().Be(firstTrainerFullName);
        couple.SecondTrainerFullName.Should().Be(secondTrainerFullName);
    }

    [Theory, AutoMoqData]
    public void Couple_WithoutTrainers_ShouldBeValid(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName secondParticipantFullName,
        CoupleDanceOrganizationName danceOrganizationName)
    {
        // Arrange & Act
        var couple = new Couple(
            id,
            tournamentId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName: null,
            secondTrainerFullName: null
        );

        // Assert
        couple.FirstParticipantFullName.Should().Be(firstParticipantFullName);
        couple.SecondParticipantFullName.Should().Be(secondParticipantFullName);
        couple.DanceOrganizationName.Should().Be(danceOrganizationName);
        couple.FirstTrainerFullName.Should().BeNull();
        couple.SecondTrainerFullName.Should().BeNull();
    }
}
