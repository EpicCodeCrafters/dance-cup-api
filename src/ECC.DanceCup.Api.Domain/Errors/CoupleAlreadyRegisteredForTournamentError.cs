using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Errors;

/// <summary>
/// Доменная ошибка: пара уже зарегистрирована на турнир
/// </summary>
public class CoupleAlreadyRegisteredForTournamentError : DomainError
{
    public CoupleAlreadyRegisteredForTournamentError()
        : base("Пара уже зарегистрирована на турнир")
    {
    }
}