using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

public readonly record struct TournamentId : IValueObject<TournamentId, long>
{
    private TournamentId(long value)
    {
        Value = value;
    }

    public static TournamentId Empty => new(default);

    public long Value { get; }

    public static TournamentId? From(long value)
    {
        throw new NotImplementedException();
    }
}