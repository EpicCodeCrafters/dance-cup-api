using ECC.DanceCup.Api.Domain.Model;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;

public interface ITournamentRepository
{
    Task<Tournament?> FindAsync(TournamentId tournamentId, CancellationToken cancellationToken);
}