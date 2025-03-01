using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.UserAggregate;

/// <summary>
/// Внешний идентификатор пользователя
/// </summary>
public struct UserExternalId : IValueObject<UserExternalId, long>
{
    private UserExternalId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }

    /// <inheritdoc />
    public static UserExternalId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new UserExternalId(value);
    }
}