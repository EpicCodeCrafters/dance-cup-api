using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

public readonly record struct TournamentDate : IValueObject<TournamentDate, DateTime>
{
    private TournamentDate(DateTime value)
    {
        Value = value;
    }
    
    public static TournamentDate Today => new(DateTime.UtcNow.Date);

    public DateTime Value { get; }

    public static TournamentDate? From(DateTime value)
    {
        return new TournamentDate(value.Date);
    }
}