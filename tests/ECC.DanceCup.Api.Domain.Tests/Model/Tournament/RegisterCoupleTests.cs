using AutoFixture;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class RegisterCoupleTests
{
    [Theory, AutoMoqData]
    public void Invoke_ShouldGenerallySuccess(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress
        );

        var categoriesIds = tournament.Categories
            .Select(category => category.Id)
            .ToArray();

        // Act

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            categoriesIds
        );

        // Assert
        
        result.ShouldBeSuccess();
    }
    
    [Theory]
    [InlineAutoMoqData(TournamentState.Created)]
    [InlineAutoMoqData(TournamentState.RegistrationFinished)]
    [InlineAutoMoqData(TournamentState.InProgress)]
    [InlineAutoMoqData(TournamentState.Finished)]
    public void Invoke_RegistrationNotInProgress_ShouldFail(
        TournamentState state,
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: state
        );

        var categoriesIds = tournament.Categories
            .Select(category => category.Id)
            .ToArray();

        // Act

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            categoriesIds
        );

        // Assert
        
        result.ShouldBeFailWith<TournamentShouldBeInStatusError>();
    }

    [Theory, AutoMoqData]
    public void Invoke_CoupleAlreadyRegistered_ShouldFail(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress
        );

        var categoriesIds = tournament.Categories
            .Select(category => category.Id)
            .ToArray();

        // Register couple first time
        tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            categoriesIds
        );

        // Act - try to register same couple again

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            categoriesIds
        );

        // Assert
        
        result.ShouldBeFailWith<CoupleAlreadyRegisteredForTournamentError>();
    }

    [Theory, AutoMoqData]
    public void Invoke_EmptyCategories_ShouldFail(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress
        );

        var emptyCategoriesIds = Array.Empty<CategoryId>();

        // Act

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            emptyCategoriesIds
        );

        // Assert
        
        result.ShouldBeFailWith<CoupleCategoriesShouldNotBeEmptyError>();
    }

    [Theory, AutoMoqData]
    public void Invoke_CategoryNotFound_ShouldFail(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        CategoryId nonExistentCategoryId,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress
        );

        var categoriesIds = new[] { nonExistentCategoryId };

        // Act

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName,
            danceOrganizationName,
            firstTrainerFullName,
            secondTrainerFullName,
            categoriesIds
        );

        // Assert
        
        result.ShouldBeFailWith<CategoriesNotFountError>();
    }

    [Theory, AutoMoqData]
    public void Invoke_WithSoloParticipant_ShouldSuccess(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: TournamentState.RegistrationInProgress
        );

        var categoriesIds = tournament.Categories
            .Select(category => category.Id)
            .ToArray();

        // Act

        var result = tournament.RegisterCouple(
            coupleId,
            firstParticipantFullName,
            secondParticipantFullName: null,
            danceOrganizationName: null,
            firstTrainerFullName: null,
            secondTrainerFullName: null,
            categoriesIds
        );

        // Assert
        
        result.ShouldBeSuccess();
    }
}