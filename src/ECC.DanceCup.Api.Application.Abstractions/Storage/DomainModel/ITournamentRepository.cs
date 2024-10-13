using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;

public interface ITournamentRepository
{
    Task<Tournament?> FindAsync(TournamentId tournamentId, CancellationToken cancellationToken);

    Task<TournamentId> AddAsync(Tournament tournament, CancellationToken cancellationToken);

    Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken);
}