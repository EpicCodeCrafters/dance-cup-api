using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.UserAggregate;

/// <summary>
/// Имя пользователя
/// </summary>
public struct Username : IValueObject<Username, string>
{
    private Username(string value)
    {
        Value = value;
    }
    
    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static Username? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new Username(value);
    }
}