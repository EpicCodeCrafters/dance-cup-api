﻿using System.Text.RegularExpressions;
using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Название турнира
/// </summary>
public readonly record struct TournamentName : IValueObject<TournamentName, string>
{
    private static readonly Regex Regex = new(@"^(?!\s*$)[\p{L}0-9 _\-,.]+$", RegexOptions.Compiled);

    private TournamentName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static TournamentName? From(string value)
    {
        if (Regex.IsMatch(value) is false)
        {
            return null;
        }

        return new TournamentName(value);
    }
}