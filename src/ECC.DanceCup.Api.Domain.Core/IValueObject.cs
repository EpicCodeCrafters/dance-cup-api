namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Объект значения
/// </summary>
/// <typeparam name="TValueObject">Тип объекта значения</typeparam>
public interface IValueObject<TValueObject>
    where TValueObject : struct;

/// <summary>
/// Объект значения
/// </summary>
/// <typeparam name="TValueObject">Тип объекта значения</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
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