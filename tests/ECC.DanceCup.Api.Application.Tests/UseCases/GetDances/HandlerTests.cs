using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases.GetDances;

public class HandlerTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        IReadOnlyCollection<DanceView> dances,
        [Frozen] Mock<IDanceViewRepository> danceViewRepositoryMock,
        GetDancesUseCase.QueryHandler handler)
    {
        // Arrange

        danceViewRepositoryMock
            .Setup(danceViewRepository => danceViewRepository.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dances);

        var query = new GetDancesUseCase.Query();

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Dances.Should().BeEquivalentTo(dances);
        
        danceViewRepositoryMock.Verify(
            danceViewRepository => danceViewRepository.FindAllAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        danceViewRepositoryMock.VerifyNoOtherCalls();
    }
}