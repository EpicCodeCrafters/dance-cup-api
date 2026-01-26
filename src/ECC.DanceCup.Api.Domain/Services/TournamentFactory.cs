using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create(UserId userId, TournamentName name, TournamentDescription description, TournamentDate date, IReadOnlyCollection<CreateCategoryModel> createCategoryModels)
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
                    refereesIds: createCategoryModel.RefereesIds.ToList(),
                    couplesIds: []
                )
            ).ToList();

        var tournament = new Tournament(
            id: TournamentId.Empty,
            version: AggregateVersion.Default,
            createdAt: now,
            changedAt: now,
            userId: userId,
            name: name,
            description: description,
            date: date,
            state: TournamentState.Created,
            registrationStartedAt: null,
            registrationFinishedAt: null,
            startedAt: null,
            finishedAt: null,
            categories: categories,
            couples: [],
            attachments: []
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