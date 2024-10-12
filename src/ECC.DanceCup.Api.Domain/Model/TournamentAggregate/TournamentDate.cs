using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Дата турнира
/// </summary>
public readonly record struct TournamentDate : IValueObject<TournamentDate, DateTime>
{
    private TournamentDate(DateTime value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public DateTime Value { get; }

    /// <inheritdoc />
    public static TournamentDate? From(DateTime value)
    {
        return new TournamentDate(value.Date);
    }
}