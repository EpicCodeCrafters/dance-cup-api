using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class TournamentRepository : ITournamentRepository
{
    public async Task<Tournament?> FindAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        return null;
    }

    public async Task<TournamentId> AddAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}