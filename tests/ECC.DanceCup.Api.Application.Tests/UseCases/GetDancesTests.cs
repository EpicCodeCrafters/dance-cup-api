using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Caching;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class GetDancesTests
{
    [Theory, AutoMoqData]
    public async Task Handle_DancesNotCached_ShouldSuccess(
        IReadOnlyCollection<DanceView> dances,
        [Frozen] Mock<IDanceViewCache> danceViewCacheMock,
        [Frozen] Mock<IDanceViewRepository> danceViewRepositoryMock,
        GetDancesUseCase.QueryHandler handler)
    {
        // Arrange

        danceViewCacheMock
            .Setup(danceViewCache => danceViewCache.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyCollection<DanceView>?)null);

        danceViewRepositoryMock
            .Setup(danceViewRepository => danceViewRepository.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dances);

        var query = new GetDancesUseCase.Query();

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Dances.Should().BeEquivalentTo(dances);
        
        danceViewCacheMock.Verify(
            danceViewCache => danceViewCache.FindAllAsync(It.IsAny<CancellationToken>()), 
            Times.Once
        );
        danceViewCacheMock.Verify(
            danceViewCache => danceViewCache.InsertRangeAsync(dances, It.IsAny<CancellationToken>()), 
            Times.Once
        );
        danceViewCacheMock.VerifyNoOtherCalls();
        
        danceViewRepositoryMock.Verify(
            danceViewRepository => danceViewRepository.FindAllAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        danceViewRepositoryMock.VerifyNoOtherCalls();
    }
    
    [Theory, AutoMoqData]
    public async Task Handle_DancesCached_ShouldSuccess(
        IReadOnlyCollection<DanceView> dances,
        [Frozen] Mock<IDanceViewCache> danceViewCacheMock,
        [Frozen] Mock<IDanceViewRepository> danceViewRepositoryMock,
        GetDancesUseCase.QueryHandler handler)
    {
        // Arrange

        danceViewCacheMock
            .Setup(danceViewCache => danceViewCache.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dances);

        var query = new GetDancesUseCase.Query();

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Dances.Should().BeEquivalentTo(dances);
        
        danceViewCacheMock.Verify(
            danceViewCache => danceViewCache.FindAllAsync(It.IsAny<CancellationToken>()), 
            Times.Once
        );
        danceViewCacheMock.VerifyNoOtherCalls();

        danceViewRepositoryMock.VerifyNoOtherCalls();
    }
}