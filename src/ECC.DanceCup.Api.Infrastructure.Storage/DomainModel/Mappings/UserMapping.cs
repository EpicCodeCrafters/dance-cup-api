using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class UserMapping
{
    public static UserDbo ToDbo(this User user)
    {
        return new UserDbo
        {
            Id = user.Id.Value,
            Version = user.Version.Value,
            CreatedAt = user.CreatedAt,
            ChangedAt = user.ChangedAt,
            ExternalId = user.ExternalId.Value,
            Username = user.Username.Value
        };
    }

    public static User ToInternal(this UserDbo user)
    {
        return new User(
            id: UserId.From(user.Id).AsRequired(),
            version: AggregateVersion.From(user.Version).AsRequired(),
            createdAt: user.CreatedAt,
            changedAt: user.ChangedAt,
            externalId: UserExternalId.From(user.ExternalId).AsRequired(),
            username: Username.From(user.Username).AsRequired()
        );
    }
}