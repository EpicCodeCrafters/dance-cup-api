using System.Reflection;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class CategoryTests
{
    [Theory, AutoMoqData]
    public void Category_ShouldHaveCorrectProperties(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds)
    {
        // Arrange & Act
        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Assert
        category.Id.Should().Be(id);
        category.TournamentId.Should().Be(tournamentId);
        category.Name.Should().Be(name);
        category.DancesIds.Should().BeEquivalentTo(dancesIds);
        category.RefereesIds.Should().BeEquivalentTo(refereesIds);
        category.CouplesIds.Should().BeEmpty();
    }

    [Theory, AutoMoqData]
    public void DancesCount_ShouldReturnCorrectCount(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<RefereeId> refereesIds)
    {
        // Arrange
        var dancesIds = new List<DanceId>
        {
            DanceId.From(1).Value,
            DanceId.From(2).Value,
            DanceId.From(3).Value
        };

        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Act & Assert
        category.DancesCount.Should().Be(3);
    }

    [Theory, AutoMoqData]
    public void RefereesCount_ShouldReturnCorrectCount(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds)
    {
        // Arrange
        var refereesIds = new List<RefereeId>
        {
            RefereeId.From(1).Value,
            RefereeId.From(2).Value
        };

        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Act & Assert
        category.RefereesCount.Should().Be(2);
    }

    [Theory, AutoMoqData]
    public void CouplesCount_WhenEmpty_ShouldReturnZero(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds)
    {
        // Arrange
        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Act & Assert
        category.CouplesCount.Should().Be(0);
    }

    [Theory, AutoMoqData]
    public void RegisterCouple_WhenNotRegistered_ShouldSuccess(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds,
        CoupleId coupleId)
    {
        // Arrange
        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Act
        var registerCoupleMethod = typeof(Category).GetMethod("RegisterCouple", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (FluentResults.Result)registerCoupleMethod!.Invoke(category, new object[] { coupleId })!;

        // Assert
        result.ShouldBeSuccess();
        category.CouplesCount.Should().Be(1);
        category.CouplesIds.Should().Contain(coupleId);
    }

    [Theory, AutoMoqData]
    public void RegisterCouple_WhenAlreadyRegistered_ShouldFail(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds,
        CoupleId coupleId)
    {
        // Arrange
        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId> { coupleId }
        );

        // Act
        var registerCoupleMethod = typeof(Category).GetMethod("RegisterCouple", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (FluentResults.Result)registerCoupleMethod!.Invoke(category, new object[] { coupleId })!;

        // Assert
        result.ShouldBeFailWith<CoupleAlreadyRegisteredInCategoryError>();
        category.CouplesCount.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void RegisterCouple_MultipleCouples_ShouldIncrementCount(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds,
        CoupleId coupleId1,
        CoupleId coupleId2,
        CoupleId coupleId3)
    {
        // Arrange
        var category = new Category(
            id,
            tournamentId,
            name,
            dancesIds,
            refereesIds,
            new List<CoupleId>()
        );

        // Act
        var registerCoupleMethod = typeof(Category).GetMethod("RegisterCouple", BindingFlags.NonPublic | BindingFlags.Instance);
        registerCoupleMethod!.Invoke(category, new object[] { coupleId1 });
        registerCoupleMethod!.Invoke(category, new object[] { coupleId2 });
        registerCoupleMethod!.Invoke(category, new object[] { coupleId3 });

        // Assert
        category.CouplesCount.Should().Be(3);
        category.CouplesIds.Should().Contain(new[] { coupleId1, coupleId2, coupleId3 });
    }
}
