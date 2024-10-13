using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

/// <summary>
/// Полное имя судьи
/// </summary>
public readonly record struct RefereeFullName : IValueObject<RefereeFullName, string>
{
    private RefereeFullName(string value)
    {
        Value = value;
    }
    
    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static RefereeFullName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new RefereeFullName(value);
    }
}