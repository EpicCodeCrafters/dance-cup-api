using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;

public interface IRefereeRepository
{
    Task<RefereeId> AddAsync(Referee referee, CancellationToken cancellationToken);
}