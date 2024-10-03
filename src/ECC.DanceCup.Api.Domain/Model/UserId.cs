using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

public readonly record struct UserId : IValueObject<UserId, long>
{
    private UserId(long value)
    {
        Value = value;
    }

    public long Value { get; }
    
    public static UserId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new UserId(value);
    }
}