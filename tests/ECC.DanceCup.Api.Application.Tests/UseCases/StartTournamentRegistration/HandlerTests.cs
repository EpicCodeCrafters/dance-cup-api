using AutoFixture;
using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Errors;
using ECC.DanceCup.Api.Application.UseCases.StartTournamentRegistration;
using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases.StartTournamentRegistration;

public class HandlerTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        TournamentId tournamentId,
        IFixture fixture,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        StartTournamentRegistrationUseCase.CommandHandler handler)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            id: tournamentId,
            state: TournamentState.Created
        );

        tournamentRepositoryMock
            .Setup(tournamentRepository => tournamentRepository.FindAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournament);

        var command = new StartTournamentRegistrationUseCase.Command(tournamentId);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeSuccess();
        
        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.FindAsync(tournamentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.UpdateAsync(tournament, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.VerifyNoOtherCalls();
    }
    
    [Theory, AutoMoqData]
    public async Task Handle_TournamentNotFound_ShouldFail(
        TournamentId tournamentId,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        StartTournamentRegistrationUseCase.CommandHandler handler)
    {
        // Arrange
        
        tournamentRepositoryMock
            .Setup(tournamentRepository => tournamentRepository.FindAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tournament?)null);

        var command = new StartTournamentRegistrationUseCase.Command(tournamentId);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeFailWith<TournamentNotFoundError>();
        
        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.FindAsync(tournamentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.VerifyNoOtherCalls();
    }

}