﻿using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.UseCases.CreateTournament;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Errors;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class CreateTournamentTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCategoryModels,
        TournamentId tournamentId,
        Tournament tournament,
        [Frozen] Mock<ITournamentFactory> tournamentFactoryMock,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        CreateTournamentUseCase.CommandHandler handler)
    {
        // Arrange

        tournamentFactoryMock
            .Setup(tournamentFactory => tournamentFactory.Create(userId, name, description, date, createCategoryModels))
            .Returns(tournament);

        tournamentRepositoryMock
            .Setup(tournamentRepository => tournamentRepository.InsertAsync(tournament, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournamentId);

        var command = new CreateTournamentUseCase.Command(userId, name, description, date, createCategoryModels);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeSuccess();
        result.Value.TournamentId.Should().Be(tournamentId);
        
        tournamentFactoryMock.Verify(
            tournamentFactory => tournamentFactory.Create(userId, name, description, date, createCategoryModels),
            Times.Once
        );
        tournamentFactoryMock.VerifyNoOtherCalls();
        
        tournamentRepositoryMock.Verify(
            tournamentRepository => tournamentRepository.InsertAsync(tournament, It.IsAny<CancellationToken>()),
            Times.Once
        );
        tournamentRepositoryMock.VerifyNoOtherCalls();
    }
    
    [Theory, AutoMoqData]
    public async Task Handle_TournamentFactoryCreateFailed_ShouldFail(
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCategoryModels,
        [Frozen] Mock<ITournamentFactory> tournamentFactoryMock,
        [Frozen] Mock<ITournamentRepository> tournamentRepositoryMock,
        CreateTournamentUseCase.CommandHandler handler)
    {
        // Arrange

        tournamentFactoryMock
            .Setup(tournamentFactory => tournamentFactory.Create(userId, name, description, date, createCategoryModels))
            .Returns(new TestError());

        var command = new CreateTournamentUseCase.Command(userId, name, description, date, createCategoryModels);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeFailWith<TestError>();
        
        tournamentFactoryMock.Verify(
            tournamentFactory => tournamentFactory.Create(userId, name, description, date, createCategoryModels),
            Times.Once
        );
        tournamentFactoryMock.VerifyNoOtherCalls();
        
        tournamentRepositoryMock.VerifyNoOtherCalls();
    }
}