using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

public readonly record struct TournamentName : IValueObject<TournamentName, string>
{
    private TournamentName(string value)
    {
        Value = value;
    }

    public string Value { get; }
    
    public static TournamentName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new TournamentName(value);
    }
}