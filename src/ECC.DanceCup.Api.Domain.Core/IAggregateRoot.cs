namespace ECC.DanceCup.Api.Domain.Core;

/// <summary>
/// Корень аггрегации
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public interface IAggregateRoot<TKey> : IEntity<TKey>;