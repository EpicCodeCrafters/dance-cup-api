using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.UseCases.CreateReferee;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Errors;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class CreateRefereeTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        RefereeFullName fullName,
        RefereeId refereeId,
        Referee referee,
        [Frozen] Mock<IRefereeFactory> refereeFactoryMock,
        [Frozen] Mock<IRefereeRepository> refereeRepositoryMock,
        CreateRefereeUseCase.CommandHandler handler)
    {
        // Arrange

        refereeFactoryMock
            .Setup(tournamentFactory => tournamentFactory.Create(fullName))
            .Returns(referee);

        refereeRepositoryMock
            .Setup(tournamentRepository => tournamentRepository.AddAsync(referee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refereeId);

        var command = new CreateRefereeUseCase.Command(fullName);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeSuccess();
        result.Value.RefereeId.Should().Be(refereeId);
        
        refereeFactoryMock.Verify(
            refereeFactory => refereeFactory.Create(fullName),
            Times.Once
        );
        refereeFactoryMock.VerifyNoOtherCalls();
        
        refereeRepositoryMock.Verify(
            refereeRepository => refereeRepository.AddAsync(referee, It.IsAny<CancellationToken>()),
            Times.Once
        );
        refereeRepositoryMock.VerifyNoOtherCalls();
    }
    
    [Theory, AutoMoqData]
    public async Task Handle_TournamentFactoryCreateFailed_ShouldFail(
        RefereeFullName fullName,
        [Frozen] Mock<IRefereeFactory> refereeFactoryMock,
        [Frozen] Mock<IRefereeRepository> refereeRepositoryMock,
        CreateRefereeUseCase.CommandHandler handler)
    {
        // Arrange

        refereeFactoryMock
            .Setup(refereeFactory => refereeFactory.Create(fullName))
            .Returns(new TestError());

        var command = new CreateRefereeUseCase.Command(fullName);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.ShouldBeFailWith<TestError>();
        
        refereeFactoryMock.Verify(
            refereeFactory => refereeFactory.Create(fullName),
            Times.Once
        );
        refereeFactoryMock.VerifyNoOtherCalls();
        
        refereeRepositoryMock.VerifyNoOtherCalls();
    }
}