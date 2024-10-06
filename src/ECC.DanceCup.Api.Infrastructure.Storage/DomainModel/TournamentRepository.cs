using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class TournamentRepository : ITournamentRepository
{
    public Task<Tournament?> FindAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        return Task.FromResult((Tournament?)null);
    }

    public Task<TournamentId> AddAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        return Task.FromResult(TournamentId.Empty);
    }

    public Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}