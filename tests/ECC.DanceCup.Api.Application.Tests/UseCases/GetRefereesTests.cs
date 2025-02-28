using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.GetReferees;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class GetRefereesTests
{

    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        RefereeFullName refereeFullName,
        int pageNumber,
        int pageSize,
        IReadOnlyCollection<RefereeView> referees,
        [Frozen] Mock<IRefereeViewRepository> refereeViewRepositoryMock,
        GetRefereesUseCase.QueryHandler handler)
    {
        // Arrange

        refereeViewRepositoryMock
            .Setup(refereeViewRepository => refereeViewRepository.FindAllAsync(refereeFullName, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(referees);

        var query = new GetRefereesUseCase.Query(refereeFullName, pageNumber, pageSize);

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Referees.Should().BeEquivalentTo(referees);

        refereeViewRepositoryMock.Verify(
            refereeViewRepository => refereeViewRepository.FindAllAsync(refereeFullName, pageNumber, pageSize, It.IsAny<CancellationToken>()),
            Times.Once
        );
        refereeViewRepositoryMock.VerifyNoOtherCalls();
    }
}