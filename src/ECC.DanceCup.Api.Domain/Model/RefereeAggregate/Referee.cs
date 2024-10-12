using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

/// <summary>
/// Судья
/// </summary>
public class Referee : AggregateRoot<RefereeId>
{
    public Referee(
        RefereeId id,
        AggregateVersion version,
        DateTime createdAt,
        DateTime changedAt,
        RefereeFullName fullName
    ) : base(id, version, createdAt, changedAt)
    {
        FullName = fullName;
    }
    
    /// <summary>
    /// Полное имя судьи
    /// </summary>
    public RefereeFullName FullName { get; }
}