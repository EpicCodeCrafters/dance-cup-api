using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Название категории
/// </summary>
public readonly record struct CategoryName : IValueObject<CategoryName, string>
{
    private CategoryName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public static CategoryName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new CategoryName(value);
    }
}

