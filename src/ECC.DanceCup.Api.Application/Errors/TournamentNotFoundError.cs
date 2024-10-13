using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Errors;

public class TournamentNotFoundError : NotFoundError
{
    public TournamentNotFoundError(TournamentId tournamentId)
        : base($"Турнир с идентификатором {tournamentId.Value} не найден")
    {
    }
}