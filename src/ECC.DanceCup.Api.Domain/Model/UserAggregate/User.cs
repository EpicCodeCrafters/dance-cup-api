using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.UserAggregate;

/// <summary>
/// Пользователь
/// </summary>
public class User : AggregateRoot<UserId>
{
    public User(
        UserId id, 
        AggregateVersion version, 
        DateTime createdAt,
        DateTime changedAt, 
        UserExternalId externalId, 
        Username username
    ) : base(id, version, createdAt, changedAt)
    {
        ExternalId = externalId;
        Username = username;
    }
    
    /// <summary>
    /// Внешний идентификатор пользователя
    /// </summary>
    public UserExternalId ExternalId { get; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public Username Username { get; }
}