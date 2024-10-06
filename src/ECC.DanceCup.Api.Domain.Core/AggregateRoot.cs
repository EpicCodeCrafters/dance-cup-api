namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Базовый класс для корней агрегации
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    where TKey : notnull
{
    protected AggregateRoot(
        TKey id,
        int version,
        DateTime createdAt,
        DateTime changedAt
    ) : base(id)
    {
        Version = version;
        CreatedAt = createdAt;
        ChangedAt = changedAt;
    }

    /// <inheritdoc />
    public int Version { get; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; }
    
    /// <inheritdoc />
    public DateTime ChangedAt { get; private set; }
    
    /// <summary>
    /// Регистрирует изменение
    /// </summary>
    protected void RegisterChange()
    {
        ChangedAt = DateTime.UtcNow;
    }
}