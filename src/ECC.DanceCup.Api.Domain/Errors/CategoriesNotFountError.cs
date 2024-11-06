using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Domain.Errors;

/// <summary>
/// Доменная ошибка: турнир не содержит определённые категории
/// </summary>
public class CategoriesNotFountError : DomainError
{
    public CategoriesNotFountError(IReadOnlyCollection<CategoryId> categoriesIds)
        : base($"Турнир не содержит категории {string.Join(", ", categoriesIds.Select(categoryId => categoryId.Value))}")
    {
    }
}