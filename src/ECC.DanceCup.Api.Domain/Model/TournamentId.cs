using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Идентификатор турнира
/// </summary>
public readonly record struct TournamentId : IValueObject<TournamentId, long>
{
    public TournamentId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Пустой идентификатор турнира
    /// </summary>
    public static TournamentId Empty => new(default);

    /// <inheritdoc />
    public long Value { get; }

    /// <inheritdoc />
    public static TournamentId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new TournamentId(value);
    }
}