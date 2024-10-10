using ECC.DanceCup.Api.Domain.Core;
using System.Text.RegularExpressions;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Название категории
/// </summary>
public readonly record struct CategoryName : IValueObject<CategoryName, string>
{
    private static readonly Regex _regex = new(@"^(?!\s*$)[\p{L}0-9 _\-,.]+$", RegexOptions.Compiled);

    private CategoryName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public static CategoryName? From(string value)
    {
        if (_regex.IsMatch(value) is false)
        {
            return null;
        }

        return new CategoryName(value);
    }
}

