namespace DanceCup.Api.Domain.Core;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : notnull
{
    protected Entity(
        TKey id, 
        DateTime createdAt, 
        DateTime changedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        ChangedAt = changedAt;
    }
    
    /// <inheritdoc />
    public TKey Id { get; }
    
    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedAt { get; }
    
    /// <summary>
    /// Время последнего изменения
    /// </summary>
    public DateTime ChangedAt { get; private set; }

    /// <summary>
    /// Регистрирует изменение
    /// </summary>
    protected void RegisterChange()
    {
        ChangedAt = DateTime.UtcNow;
    }

    public bool Equals(IEntity<TKey>? other)
    {
        return other?.Id.Equals(Id) is true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TKey>);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}