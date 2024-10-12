namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Версия агрегата
/// </summary>
public struct AggregateVersion : IValueObject<AggregateVersion, int>
{
    private AggregateVersion(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Первоначальная версия агрегата
    /// </summary>
    public static AggregateVersion Default => new(1);

    /// <inheritdoc />
    public int Value { get; }
    
    /// <inheritdoc />
    public static AggregateVersion? From(int value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new AggregateVersion(value);
    }

    /// <summary>
    /// Возвращает увеличенную версию агрегата
    /// </summary>
    /// <returns></returns>
    public AggregateVersion Increase()
    {
        return new AggregateVersion(Value + 1);
    }
}