using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Domain.Errors;

/// <summary>
/// Доменная ошибка: турнир должен быть в определённом статусе
/// </summary>
public class TournamentShouldBeInStatusError : DomainError
{
    public TournamentShouldBeInStatusError(TournamentState tournamentState)
        : base($"Турнир должен быть в статусе {tournamentState}")
    {
    }
}