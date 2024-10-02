namespace DanceCup.Api.Domain.Core;

/// <summary>
/// Сущность
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public interface IEntity<TKey> : IEquatable<IEntity<TKey>>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    TKey Id { get; }
}