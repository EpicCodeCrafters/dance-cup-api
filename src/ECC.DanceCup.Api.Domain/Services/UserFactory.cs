using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <inheritdoc />
public class UserFactory : IUserFactory
{
    /// <inheritdoc />
    public Result<User> Create(UserExternalId externalId, Username username)
    {
        var now = DateTime.UtcNow;

        return new User(
            id: UserId.Empty,
            version: AggregateVersion.Default,
            createdAt: now,
            changedAt: now,
            externalId: externalId,
            username: username
        );
    }
}