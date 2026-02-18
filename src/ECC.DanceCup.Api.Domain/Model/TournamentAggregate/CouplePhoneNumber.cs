using System.Text.RegularExpressions;
using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Номер телефона пары
/// </summary>
public readonly record struct CouplePhoneNumber: IValueObject<CouplePhoneNumber, string>
{
    private static readonly Regex RuPhoneRegex = new(@"^(?:\+7|7|8)[1-9]\d{9}$", RegexOptions.Compiled);
    private CouplePhoneNumber(string value)
    {
        Value = value;
    }
    
    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static CouplePhoneNumber? From(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        var normalized = trimmed switch
        {
            var v when v.StartsWith("+7") => v,
            var v when v.StartsWith("8")  => "+7" + v[1..],
            var v when v.StartsWith("7")  => "+" + v,
            _ => trimmed
        };

        return new CouplePhoneNumber(normalized);
    }
}