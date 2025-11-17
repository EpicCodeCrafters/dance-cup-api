using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Категория
/// </summary>
public class Category : Entity<CategoryId>
{
    private readonly List<DanceId> _dancesIds;
    private readonly List<RefereeId> _refereesIds;
    private readonly List<CoupleId> _couplesIds;
    private readonly List<Round> _rounds;

    public Category(
        CategoryId id,
        TournamentId tournamentId,
        CategoryName name,
        List<DanceId> dancesIds,
        List<RefereeId> refereesIds,
        List<CoupleId> couplesIds,
        List<Round> rounds
    ) : base(id)
    {
        TournamentId = tournamentId;
        Name = name;
        _dancesIds = dancesIds;
        _refereesIds = refereesIds;
        _couplesIds = couplesIds;
        _rounds = rounds;
    }

    /// <summary>
    /// Идентификатор турнира категории
    /// </summary>
    public TournamentId TournamentId { get; }

    /// <summary>
    /// Название категории
    /// </summary>
    public CategoryName Name { get; }

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

    /// <summary>
    /// Список идентификаторов пар, участвующих в категории
    /// </summary>
    public IReadOnlyCollection<CoupleId> CouplesIds => _couplesIds;

    /// <summary>
    /// Количеситво пар, участвующих в категории
    /// </summary>
    public int CouplesCount => _couplesIds.Count;

    /// <summary>
    /// Список раундов категории
    /// </summary>
    public IReadOnlyCollection<Round> Rounds => _rounds;

    /// <summary>
    /// Количество раундов категории
    /// </summary>
    public int RoundsCount => _rounds.Count;

    internal Result RegisterCouple(CoupleId coupleId)
    {
        if (_couplesIds.Contains(coupleId))
        {
            return new CoupleAlreadyRegisteredInCategoryError();
        }
        
        _couplesIds.Add(coupleId);
        
        return Result.Ok();
    }
}