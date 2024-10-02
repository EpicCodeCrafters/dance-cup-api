namespace DanceCup.Api.Domain.Core;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    where TKey : notnull
{
    protected AggregateRoot(
        TKey id,
        DateTime createdAt,
        DateTime changedAt)
        : base(id, createdAt, changedAt)
    {
    }
}