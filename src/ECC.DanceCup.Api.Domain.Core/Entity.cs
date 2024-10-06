namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Базовый класс для сущностей
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : notnull
{
    protected Entity(TKey id)
    {
        Id = id;
    }
    
    /// <inheritdoc />
    public TKey Id { get; }

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