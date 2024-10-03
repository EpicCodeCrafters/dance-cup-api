namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValueObject"></typeparam>
public interface IValueObject<TValueObject>
    where TValueObject : struct;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValueObject"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IValueObject<TValueObject, TValue> : IValueObject<TValueObject>
    where TValueObject : struct
{
    /// <summary>
    /// Значение
    /// </summary>
    TValue Value { get; }

    /// <summary>
    /// Возвращает корректный тип значения или null
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    static abstract TValueObject? From(TValue value);
}