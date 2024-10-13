using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Domain.Error;

/// <summary>
/// Доменная ошибка: турнир должен быть в определённом статусе
/// </summary>
public class TournamentShouldBeInStatusError : DomainError
{
    public TournamentShouldBeInStatusError(TournamentId tournamentId, TournamentState tournamentState)
        : base($"Турнир с идентификатором {tournamentId.Value} должен быть в статусе {tournamentState}")
    {
    }
}