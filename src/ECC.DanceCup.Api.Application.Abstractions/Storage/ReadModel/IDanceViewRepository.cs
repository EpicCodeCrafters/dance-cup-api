using ECC.DanceCup.Api.Application.Abstractions.Models.Views;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface IDanceViewRepository
{
    Task<IReadOnlyCollection<DanceView>> FindAllAsync(CancellationToken cancellationToken);
}