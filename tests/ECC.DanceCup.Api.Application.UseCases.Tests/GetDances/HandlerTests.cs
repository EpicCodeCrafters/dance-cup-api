using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Tests.Common;
using FluentAssertions;

namespace ECC.DanceCup.Api.Application.UseCases.Tests.GetDances;

public class HandlerTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        GetDancesUseCase.QueryHandler handler)
    {
        // Arrange

        var query = new GetDancesUseCase.Query();

        // Act

        var result = await handler.Handle(query, CancellationToken.None);

        // Assert

        result.IsSuccess.Should().BeTrue();
    }

}