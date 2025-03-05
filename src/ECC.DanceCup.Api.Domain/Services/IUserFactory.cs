using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <summary>
/// Фабрика пользователей
/// </summary>
public interface IUserFactory
{
    /// <summary>
    /// Создаёт пользователя
    /// </summary>
    /// <param name="externalId">Внешний идентификатор пользователя</param>
    /// <param name="username">Имя пользователя</param>
    /// <returns></returns>
    Result<User> Create(UserExternalId externalId, Username username);
}