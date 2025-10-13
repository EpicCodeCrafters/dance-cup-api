using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.UseCases.GetTournaments;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class GetTournamentsTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        UserId userId,
        int pageNumber,
        int pageSize,
        IReadOnlyCollection<TournamentView> tournaments,
        [Frozen] Mock<ITournamentViewRepository> tournamentViewRepositoryMock,
        GetTournamentsUseCase.QueryHandler handler)
    {
        // Arrange

        tournamentViewRepositoryMock
            .Setup(repository => repository.FindAllAsync(userId, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournaments);

        var query = new GetTournamentsUseCase.Query(userId, pageNumber, pageSize);

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Tournaments.Should().BeEquivalentTo(tournaments);

        tournamentViewRepositoryMock.Verify(
            repository => repository.FindAllAsync(userId, pageNumber, pageSize, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentViewRepositoryMock.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task Handle_WithEmptyResults_ShouldReturnEmptyCollection(
        UserId userId,
        int pageNumber,
        int pageSize,
        [Frozen] Mock<ITournamentViewRepository> tournamentViewRepositoryMock,
        GetTournamentsUseCase.QueryHandler handler)
    {
        // Arrange

        var emptyTournaments = Array.Empty<TournamentView>();
        
        tournamentViewRepositoryMock
            .Setup(repository => repository.FindAllAsync(userId, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyTournaments);

        var query = new GetTournamentsUseCase.Query(userId, pageNumber, pageSize);

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Tournaments.Should().BeEmpty();

        tournamentViewRepositoryMock.Verify(
            repository => repository.FindAllAsync(userId, pageNumber, pageSize, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentViewRepositoryMock.VerifyNoOtherCalls();
    }
}
