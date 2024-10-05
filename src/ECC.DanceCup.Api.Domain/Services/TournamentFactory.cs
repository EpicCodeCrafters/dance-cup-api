using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create(UserId userId, TournamentName name, List<Category> catigories)
    {
        var tournament = new Tournament(
            id: TournamentId.Empty,
            createdAt: DateTime.UtcNow,
            changedAt: DateTime.UtcNow,
            userId: userId,
            name: name,
            date: TournamentDate.From(DateTime.UtcNow)!.Value,
            state: TournamentState.Created,
            startedAt: DateTime.UtcNow,
            finishedAt: DateTime.UtcNow,
            categories: catigories
        );

        return tournament;
    }
}