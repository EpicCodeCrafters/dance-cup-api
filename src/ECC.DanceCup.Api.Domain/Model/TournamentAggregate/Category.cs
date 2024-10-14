using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Категория
/// </summary>
public class Category : Entity<CategoryId>
{
    private readonly List<DanceId> _dancesIds;
    private readonly List<RefereeId> _refereesIds;

    public Category(
        CategoryId id,
        TournamentId tournamentId,
        TournamentDescription name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds
    ) : base(id)
    {
        TournamentId = tournamentId;
        Name = name;
        _dancesIds = dancesIds;
        _refereesIds = refereesIds;
    }

    /// <summary>
    /// Идентификатор турнира категории
    /// </summary>
    public TournamentId TournamentId { get; }

    /// <summary>
    /// Название категории
    /// </summary>
    public TournamentDescription Name { get; }

    /// <summary>
    /// Список идентификаторов танцев категории
    /// </summary>
    public IReadOnlyCollection<DanceId> DancesIds => _dancesIds;

    /// <summary>
    /// Количество танцев категории 
    /// </summary>
    public int DancesCount => _dancesIds.Count;

    /// <summary>
    /// Список идентификаторов судей категории
    /// </summary>
    public IReadOnlyCollection<RefereeId> RefereesIds => _refereesIds;

    /// <summary>
    /// Количество судей категории
    /// </summary>
    public int RefereesCount => _refereesIds.Count;
}