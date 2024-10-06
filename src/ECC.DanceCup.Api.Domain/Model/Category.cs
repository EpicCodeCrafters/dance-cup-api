using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Категория
/// </summary>
public class Category : Entity<CategoryId>
{
    private readonly List<DanceId> _dancesIds;
    private readonly List<RefereeId> _refereesIds;

    public Category(
        CategoryId id,
        DateTime createdAt,
        DateTime changedAt,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds)
        : base(id, createdAt, changedAt)
    {
        _dancesIds = dancesIds;
        _refereesIds = refereesIds;
    }

    /// <summary>
    /// Список идентификаторов танцев категории
    /// </summary>
    public IReadOnlyCollection<DanceId> DancesIds => _dancesIds;

    /// <summary>
    /// Список идентификаторов судей категории
    /// </summary>
    public IReadOnlyCollection<RefereeId> RefereesIds => _refereesIds;
}