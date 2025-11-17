using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Идентификатор раунда
/// </summary>
public readonly record struct RoundId : IValueObject<RoundId, long>
{
    private RoundId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }
    
    /// <inheritdoc />
    public static RoundId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new RoundId(value);
    }
}
