using System.Text.RegularExpressions;
using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Описание турнира
/// </summary>
public readonly record struct TournamentDescription : IValueObject<TournamentDescription, string>
{
    private TournamentDescription(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public static TournamentDescription? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new TournamentDescription(value);
    }
}

