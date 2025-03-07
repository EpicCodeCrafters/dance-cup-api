using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.UserAggregate;

/// <summary>
/// Идентификатор пользователя
/// </summary>
public readonly record struct UserId : IValueObject<UserId, long>
{
    private UserId(long value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Пустой идентификатор пользователя
    /// </summary>
    public static UserId Empty => new(default);

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