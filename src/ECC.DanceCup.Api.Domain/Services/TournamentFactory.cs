using ECC.DanceCup.Api.Domain.Model;
using FluentResults;
using System.Collections.Generic;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create(UserId userId, TournamentName name, TournamentDate date, IReadOnlyCollection<CreateCategoryModel> createCategoryModels)
    {
        var now = DateTime.UtcNow;

        var categories = createCategoryModels
            .Select(
                createCategoryModel => new Category(
                    id: CategoryId.Empty,
                    createdAt: now,
                    changedAt: now,
                    tournamentId: TournamentId.Empty,
                    categoryName: createCategoryModel.CategoryName,
                    dancesIds: createCategoryModel.DancesIds.ToList(),
                    refereesIds: createCategoryModel.RefereesIds.ToList()
             )
        ).ToList();
        
        var tournament = new Tournament(
            id: TournamentId.Empty,
            createdAt: now,
            changedAt: now,
            userId: userId,
            name: name,
            date: date,
            state: TournamentState.Created,
            startedAt: null,
            finishedAt: null,
            categories:categories
            );
        return tournament;
    }
}