using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.Providers;

public interface ICoupleIdProvider
{
    Task<CoupleId> CreateNewAsync(CancellationToken cancellationToken);
}