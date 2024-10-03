using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class TournamentFactory : ITournamentFactory
{
    /// <inheritdoc />
    public Result<Tournament> Create()
    {
        return Result.Fail("Пока не сделано");
    }
}