namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Корень агрегации
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public interface IAggregateRoot<TKey> : IEntity<TKey>
{
    /// <summary>
    /// Версия
    /// </summary>
    public AggregateVersion Version { get; }
    
    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Время последнего изменения
    /// </summary>
    public DateTime ChangedAt { get; }
}