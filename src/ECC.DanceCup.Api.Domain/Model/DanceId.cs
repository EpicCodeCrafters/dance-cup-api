using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Идентификатор танца
/// </summary>
public readonly record struct DanceId : IValueObject<DanceId, long>
{
    public DanceId(long value)
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