using AutoFixture;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class TournamentFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCategoryModels,
        TournamentFactory tournamentFactory)
    {
        // Arrange

        // Act

        var result = tournamentFactory.Create(userId, name, description, date, createCategoryModels);

        // Assert

        result.ShouldBeSuccess();
    }

    [Theory, AutoMoqData]
    public void Create_WithDuplicateDanceIds_ShouldReturnFailure(
        IFixture fixture,
        TournamentFactory factory,
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        CategoryName categoryName,
        RefereeId refereeId1,
        RefereeId refereeId2)
    {
        // Arrange
        var danceId = fixture.Create<DanceId>();
        var createCategoryModels = new List<CreateCategoryModel>
        {
            new(categoryName, [danceId, danceId], [refereeId1, refereeId2]) // Duplicate dance IDs
        };

        // Act
        var result = factory.Create(userId, name, description, date, createCategoryModels);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
    }

    [Theory, AutoMoqData]
    public void Create_WithDuplicateRefereeIds_ShouldReturnFailure(
        IFixture fixture,
        TournamentFactory factory,
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        CategoryName categoryName,
        DanceId danceId1,
        DanceId danceId2)
    {
        // Arrange
        var refereeId = fixture.Create<RefereeId>();
        var createCategoryModels = new List<CreateCategoryModel>
        {
            new(categoryName, [danceId1, danceId2], [refereeId, refereeId]) // Duplicate referee IDs
        };

        // Act
        var result = factory.Create(userId, name, description, date, createCategoryModels);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
    }

    [Theory, AutoMoqData]
    public void Create_WithMultipleCategoriesWithDuplicates_ShouldReturnFailure(
        TournamentFactory factory,
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        CategoryName categoryName1,
        CategoryName categoryName2,
        DanceId danceId1,
        DanceId danceId2,
        RefereeId refereeId)
    {
        // Arrange
        var createCategoryModels = new List<CreateCategoryModel>
        {
            new(categoryName1, [danceId1, danceId2], [refereeId]),
            new(categoryName2, [danceId1, danceId1], [refereeId, refereeId]) // Both duplicates
        };

        // Act
        var result = factory.Create(userId, name, description, date, createCategoryModels);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
    }

    [Theory, AutoMoqData]
    public void Create_WithValidMultipleCategories_ShouldReturnSuccess(
        TournamentFactory factory,
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        CategoryName categoryName1,
        CategoryName categoryName2,
        DanceId danceId1,
        DanceId danceId2,
        DanceId danceId3,
        RefereeId refereeId1,
        RefereeId refereeId2)
    {
        // Arrange
        var createCategoryModels = new List<CreateCategoryModel>
        {
            new(categoryName1, [danceId1, danceId2], [refereeId1]),
            new(categoryName2, [danceId2, danceId3], [refereeId1, refereeId2])
        };

        // Act
        var result = factory.Create(userId, name, description, date, createCategoryModels);

        // Assert
        result.ShouldBeSuccess();
        result.Value.Categories.Should().HaveCount(2);
        var categoriesList = result.Value.Categories.ToList();
        categoriesList[0].DancesIds.Should().HaveCount(2);
        categoriesList[1].DancesIds.Should().HaveCount(2);
    }
}