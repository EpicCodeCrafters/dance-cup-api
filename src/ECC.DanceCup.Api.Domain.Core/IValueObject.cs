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
public interface IValueObject<TValueObject, out TValue> : IValueObject<TValueObject>
    where TValueObject : struct
{
    /// <summary>
    /// Значение
    /// </summary>
    TValue Value { get; }
}