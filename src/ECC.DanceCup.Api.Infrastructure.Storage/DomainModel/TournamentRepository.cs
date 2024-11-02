using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class TournamentRepository : ITournamentRepository
{
    private readonly Dictionary<TournamentId, Tournament> _tournaments = new();

    public Task<TournamentId> InsertAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        var tournamentId = TournamentId.From(_tournaments.Count + 1).AsRequired();
        
        _tournaments.Add(tournamentId, tournament);
        
        return Task.FromResult(tournamentId);
    }

    public Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        _tournaments[tournament.Id] = tournament;
        
        return Task.CompletedTask;
    }
    
    public Task<Tournament?> FindByIdAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        var tournament = _tournaments.GetValueOrDefault(tournamentId);

        return Task.FromResult(tournament);
    }
}