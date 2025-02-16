using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface ITournamentRegistrationResultViewRepository
{
    Task<IReadOnlyCollection<TournamentRegistrationResultView>> FindAllAsync(TournamentId tournamentId ,CancellationToken cancellationToken);
}