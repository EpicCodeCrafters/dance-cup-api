using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Порядковый номер раунда
/// </summary>
public readonly record struct RoundOrderNumber : IValueObject<RoundOrderNumber, int>
{
    private RoundOrderNumber(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public int Value { get; }
    
    /// <inheritdoc />
    public static RoundOrderNumber? From(int value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new RoundOrderNumber(value);
    }
}
