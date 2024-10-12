using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class RefereeMapping
{
    public static RefereeDbo ToDbo(this Referee referee)
    {
        return new RefereeDbo
        {
            Id = referee.Id.Value,
            Version = referee.Version.Value,
            CreatedAt = referee.CreatedAt,
            ChangedAt = referee.ChangedAt,
            FullName = referee.FullName.Value
        };
    }

    public static Referee ToInternal(this RefereeDbo referee)
    {
        return new Referee(
            id: RefereeId.From(referee.Id).AsRequired(),
            version: AggregateVersion.From(referee.Version).AsRequired(),
            createdAt: referee.CreatedAt,
            changedAt: referee.ChangedAt,
            fullName: RefereeFullName.From(referee.FullName).AsRequired()
        );
    }
}