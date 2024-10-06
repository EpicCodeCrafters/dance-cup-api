using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class TournamentFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        UserId userId,
        TournamentName name,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCatigoryModels,
        TournamentFactory tournamentFactory)
    {
        // Arrange

        // Act

        var result = tournamentFactory.Create(userId, name, date, createCatigoryModels);

        // Assert

        result.IsSuccess.Should().BeTrue();
    }
}