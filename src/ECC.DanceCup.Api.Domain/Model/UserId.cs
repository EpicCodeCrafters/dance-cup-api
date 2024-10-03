using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Идентификатор пользователя
/// </summary>
public readonly record struct UserId : IValueObject<UserId, long>
{
    private UserId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }
    
    /// <inheritdoc />
    public static UserId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new UserId(value);
    }
}