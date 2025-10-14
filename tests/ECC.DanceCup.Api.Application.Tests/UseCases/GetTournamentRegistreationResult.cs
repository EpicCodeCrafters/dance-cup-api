using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.UseCases.GetTournamentRegistrationResult;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class GetTournamentRegistreationResult
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        TournamentId tournamentId,
        IReadOnlyCollection<TournamentRegistrationResultView> resultOfRegistration,
        [Frozen] Mock<ITournamentViewRepository> tournamentViewRepositoryMock,
        GetTournamentRegistrationResultUseCase.QueryHandler handler
    )
    {
        //Arrange
        
        tournamentViewRepositoryMock.Setup(tournament =>
            tournament.GetRegistrationResultAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultOfRegistration);
        
        var query = new GetTournamentRegistrationResultUseCase.Query(tournamentId);
        
        //Act
        
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        
        result.ShouldBeSuccess();
        result.Value.ResultOfRegistration.Should().BeEquivalentTo(resultOfRegistration);
        
        tournamentViewRepositoryMock.Verify(
            tournament => tournament.GetRegistrationResultAsync(tournamentId, It.IsAny<CancellationToken>())
            ,Times.Once
        );
        
        tournamentViewRepositoryMock.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task Handle_WithEmptyResults_ShouldReturnEmptyCollection(
        TournamentId tournamentId,
        [Frozen] Mock<ITournamentViewRepository> tournamentViewRepositoryMock,
        GetTournamentRegistrationResultUseCase.QueryHandler handler)
    {
        // Arrange

        var emptyResults = Array.Empty<TournamentRegistrationResultView>();
        
        tournamentViewRepositoryMock
            .Setup(repository => repository.GetRegistrationResultAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyResults);

        var query = new GetTournamentRegistrationResultUseCase.Query(tournamentId);

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.ResultOfRegistration.Should().BeEmpty();

        tournamentViewRepositoryMock.Verify(
            repository => repository.GetRegistrationResultAsync(tournamentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentViewRepositoryMock.VerifyNoOtherCalls();
    }
}