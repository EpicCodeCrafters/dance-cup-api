﻿using System.Text.RegularExpressions;
using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Название категории
/// </summary>
public readonly record struct TournamentDescription : IValueObject<TournamentDescription, string>
{
    private static readonly Regex Regex = new(@"^(?!\s*$)[\p{L}0-9 _\-,.]+$", RegexOptions.Compiled);

    private TournamentDescription(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public static TournamentDescription? From(string value)
    {
        if (Regex.IsMatch(value) is false)
        {
            return null;
        }

        return new TournamentDescription(value);
    }
}

