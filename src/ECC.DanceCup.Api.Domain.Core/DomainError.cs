using FluentResults;

namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Доменная ошибка
/// </summary>
public abstract class DomainError : Error
{
    protected DomainError(string message)
    {
        Message = message;
    }
}