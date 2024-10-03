using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Application.Errors;

public class TournamentNotFoundError : Error
{
    public TournamentNotFoundError(TournamentId tournamentId)
    {
        Message = $"Турнир с идентификатором {tournamentId.Value} не найден";
    }
}