using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Категория
/// </summary>
public class Category : Entity<CategoryId>
{
    private readonly List<DanceId> _dancesIds;

    public Category(
        CategoryId id,
        DateTime createdAt,
        DateTime changedAt,
        List<DanceId> dancesIds)
        : base(id, createdAt, changedAt)
    {
        _dancesIds = dancesIds;
    }

    /// <summary>
    /// Список танцев категории
    /// </summary>
    public IReadOnlyCollection<DanceId> DancesIds => _dancesIds;
}