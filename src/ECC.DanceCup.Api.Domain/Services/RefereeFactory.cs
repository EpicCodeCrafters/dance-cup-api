using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

public class RefereeFactory : IRefereeFactory
{
    public Result<Referee> Create(RefereeFullName fullName)
    {
        var now = DateTime.UtcNow;

        return new Referee(
            id: RefereeId.Empty,
            version: AggregateVersion.Default, 
            createdAt: now,
            changedAt: now,
            fullName: fullName
        );
    }
}