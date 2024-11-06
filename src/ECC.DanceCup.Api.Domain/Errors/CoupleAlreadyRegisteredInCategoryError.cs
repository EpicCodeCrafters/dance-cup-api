using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Errors;

/// <summary>
/// Доменная ошибка: пара уже участвует в категории
/// </summary>
public class CoupleAlreadyRegisteredInCategoryError : DomainError
{
    public CoupleAlreadyRegisteredInCategoryError()
        : base("Пара уже участвует в категории")
    {
    }
}