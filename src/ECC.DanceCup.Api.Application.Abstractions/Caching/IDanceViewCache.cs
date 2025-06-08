using ECC.DanceCup.Api.Application.Abstractions.Models.Views;

namespace ECC.DanceCup.Api.Application.Abstractions.Caching;

public interface IDanceViewCache
{
    Task<IReadOnlyCollection<DanceView>?> FindAllAsync(CancellationToken cancellationToken);
    
    Task InsertRangeAsync(IReadOnlyCollection<DanceView> danceViews, CancellationToken cancellationToken);
}