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
        AggregateVersion version,
        DateTime createdAt,
        DateTime changedAt
    ) : base(id)
    {
        Version = version;
        CreatedAt = createdAt;
        ChangedAt = changedAt;
    }

    /// <inheritdoc />
    public AggregateVersion Version { get; private set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; }
    
    /// <inheritdoc />
    public DateTime ChangedAt { get; private set; }
    
    /// <summary>
    /// Регистрирует изменение
    /// </summary>
    protected void RegisterChange()
    {
        Version = Version.Increase();
        ChangedAt = DateTime.UtcNow;
    }
}