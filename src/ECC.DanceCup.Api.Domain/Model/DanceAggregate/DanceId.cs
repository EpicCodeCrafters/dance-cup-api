using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.DanceAggregate;

/// <summary>
/// Идентификатор танца
/// </summary>
public readonly record struct DanceId : IValueObject<DanceId, long>
{
    private DanceId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }

    /// <inheritdoc />
    public static DanceId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }
        
        return new DanceId(value);
    }
}