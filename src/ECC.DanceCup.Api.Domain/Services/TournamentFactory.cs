using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create(UserId userId, TournamentName name, TournamentDate date, IReadOnlyCollection<CreateCategoryModel> createCategoryModels)
    {
        var now = DateTime.UtcNow;

        var validateCreateCategoryModelsResult = ValidateCreateCategoryModels(createCategoryModels);
        if (validateCreateCategoryModelsResult.IsFailed)
        {
            return validateCreateCategoryModelsResult;
        }

        var categories = createCategoryModels
            .Select(
                createCategoryModel => new Category(
                    id: CategoryId.Empty,
                    tournamentId: TournamentId.Empty,
                    name: createCategoryModel.Name,
                    dancesIds: createCategoryModel.DancesIds.ToList(),
                    refereesIds: createCategoryModel.RefereesIds.ToList()
                )
            ).ToList();

        var tournament = new Tournament(
            id: TournamentId.Empty,
            version: 1,
            createdAt: now,
            changedAt: now,
            userId: userId,
            name: name,
            date: date,
            state: TournamentState.Created,
            registrationStartedAt: null,
            registrationFinishedAt: null,
            startedAt: null,
            finishedAt: null,
            categories: categories
        );
        
        return tournament;
    }

    private static Result ValidateCreateCategoryModels(IReadOnlyCollection<CreateCategoryModel> createCategoryModels)
    {
        var errors = createCategoryModels
            .Select(ValidateCreateCategoryModel)
            .SelectMany(result => result.Errors)
            .ToArray();

        if (errors.Length != 0)
        {
            return Result.Fail(errors);
        }
        
        return Result.Ok();
    }
    
    private static Result ValidateCreateCategoryModel(CreateCategoryModel createCategoryModel)
    {
        if (createCategoryModel.DancesIds.Count != createCategoryModel.DancesIds.Distinct().Count())
        {
            return Result.Fail("2");
        }
        
        if (createCategoryModel.RefereesIds.Count != createCategoryModel.RefereesIds.Distinct().Count())
        {
            return Result.Fail("2");
        }
        
        return Result.Ok();
    }
}