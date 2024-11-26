using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;

public interface ITournamentRepository
{
    Task<TournamentId> InsertAsync(Tournament tournament, List<Category> catigories, CancellationToken cancellationToken);

    Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken);
    
    Task<Tournament?> FindByIdAsync(TournamentId tournamentId, CancellationToken cancellationToken);
}