using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Идентификатор пары
/// </summary>
public readonly record struct CoupleId : IValueObject<CoupleId, long>
{
    private CoupleId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }
    
    /// <inheritdoc />
    public static CoupleId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new CoupleId(value);
    }
}