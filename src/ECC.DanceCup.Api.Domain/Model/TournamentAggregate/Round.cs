using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Раунд
/// </summary>
public class Round : Entity<RoundId>
{
    private readonly List<CoupleId> _couplesIds;

    public Round(
        RoundId id,
        CategoryId categoryId,
        RoundOrderNumber orderNumber,
        List<CoupleId> couplesIds
    ) : base(id)
    {
        CategoryId = categoryId;
        OrderNumber = orderNumber;
        _couplesIds = couplesIds;
    }

    /// <summary>
    /// Идентификатор категории раунда
    /// </summary>
    public CategoryId CategoryId { get; }

    /// <summary>
    /// Порядковый номер раунда
    /// </summary>
    public RoundOrderNumber OrderNumber { get; }

    /// <summary>
    /// Список идентификаторов пар, участвующих в раунде
    /// </summary>
    public IReadOnlyCollection<CoupleId> CouplesIds => _couplesIds;

    /// <summary>
    /// Количество пар, участвующих в раунде
    /// </summary>
    public int CouplesCount => _couplesIds.Count;
}
