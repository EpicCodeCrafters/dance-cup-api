using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create()
    {
        var tournament = new Tournament(
            id: TournamentId.Empty,
            createdAt: DateTime.UtcNow,
            changedAt: DateTime.UtcNow,
            userId: UserId.From(1)!.Value,
            name: TournamentName.From("aaa")!.Value,
            date: TournamentDate.From(DateTime.UtcNow)!.Value,
            state: TournamentState.Created,
            startedAt: DateTime.UtcNow,
            finishedAt: DateTime.UtcNow,
            categories: []
        );

        return tournament;
    }
}