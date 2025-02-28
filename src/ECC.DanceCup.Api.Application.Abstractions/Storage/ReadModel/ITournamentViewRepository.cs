using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface ITournamentViewRepository
{
    Task<IReadOnlyCollection<TournamentRegistrationResultView>> GetRegistrationResultAsync(TournamentId tournamentId, CancellationToken cancellationToken);
}