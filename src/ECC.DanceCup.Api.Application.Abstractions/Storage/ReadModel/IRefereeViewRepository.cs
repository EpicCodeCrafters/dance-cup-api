using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface IRefereeViewRepository
{
    Task<IReadOnlyCollection<RefereeView>> FindAllAsync(CancellationToken cancellationToken);
}