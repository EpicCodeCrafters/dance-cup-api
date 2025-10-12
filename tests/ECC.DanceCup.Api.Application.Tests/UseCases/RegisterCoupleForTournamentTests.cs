using AutoFixture;
using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.Providers;
using ECC.DanceCup.Api.Application.Errors;
using ECC.DanceCup.Api.Application.UseCases.RegisterCoupleForTournament;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class RegisterCoupleForTournamentTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        TournamentId tournamentId,
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IFixture fixture,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        [Frozen] Mock<ICoupleIdProvider> coupleIdProviderMock,
        RegisterCoupleForTournamentUseCase.CommandHandler handler)
    {
        // Arrange
        
        var tournament = fixture.CreateTournament(
            id: tournamentId,
            state: TournamentState.RegistrationInProgress
        );

        var categoriesIds = tournament.Categories
            .Select(category => category.Id)
            .ToArray();

        tournamentRepositoryMock
            .Setup(
                tournamentRepository => tournamentRepository.FindByIdAsync(
                    tournamentId,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(tournament);

        coupleIdProviderMock
            .Setup(coupleIdProvider => coupleIdProvider.CreateNewAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(coupleId);

        var command = new RegisterCoupleForTournamentUseCase.Command(
            tournamentId, 
            firstParticipantFullName, 
            secondParticipantFullName,
            danceOrganizationName,
            null,
            firstTrainerFullName, 
            secondTrainerFullName, 
            categoriesIds
        );

        // Act

        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        
        result.ShouldBeSuccess();

        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.FindByIdAsync(tournamentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.UpdateAsync(tournament, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.VerifyNoOtherCalls();
        
        coupleIdProviderMock.Verify(
            coupleIdProvider => coupleIdProvider.CreateNewAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        coupleIdProviderMock.VerifyNoOtherCalls();
    }
    
    [Theory, AutoMoqData]
    public async Task Handle_TournamentNotFound_ShouldFail(
        TournamentId tournamentId,
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IReadOnlyCollection<CategoryId> categoriesIds,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        [Frozen] Mock<ICoupleIdProvider> coupleIdProviderMock,
        RegisterCoupleForTournamentUseCase.CommandHandler handler)
    {
        // Arrange

        tournamentRepositoryMock
            .Setup(
                tournamentRepository => tournamentRepository.FindByIdAsync(
                    tournamentId,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync((Tournament?)null);

        coupleIdProviderMock
            .Setup(coupleIdProvider => coupleIdProvider.CreateNewAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(coupleId);

        var command = new RegisterCoupleForTournamentUseCase.Command(
            tournamentId, 
            firstParticipantFullName, 
            secondParticipantFullName,
            danceOrganizationName,
            null,
            firstTrainerFullName, 
            secondTrainerFullName, 
            categoriesIds
        );

        // Act

        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        
        result.ShouldBeFailWith<TournamentNotFoundError>();

        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.FindByIdAsync(tournamentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.VerifyNoOtherCalls();
        
        coupleIdProviderMock.VerifyNoOtherCalls();
    }
}