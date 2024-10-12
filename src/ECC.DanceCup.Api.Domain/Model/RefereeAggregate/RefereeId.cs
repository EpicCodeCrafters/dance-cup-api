using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

/// <summary>
/// Идентификатор судьи
/// </summary>
public readonly record struct RefereeId : IValueObject<RefereeId, long>
{
    private RefereeId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Пустой идентификатор судьи
    /// </summary>
    public static RefereeId Empty => new(default);

    /// <inheritdoc />
    public long Value { get; }

    /// <inheritdoc />
    public static RefereeId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new RefereeId(value);
    }
}

