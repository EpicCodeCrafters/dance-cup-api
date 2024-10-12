using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class TournamentFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        UserId userId,
        TournamentName name,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCategoryModels,
        TournamentFactory tournamentFactory)
    {
        // Arrange

        // Act

        var result = tournamentFactory.Create(userId, name, date, createCategoryModels);

        // Assert

        result.ShouldBeSuccess();
    }
}