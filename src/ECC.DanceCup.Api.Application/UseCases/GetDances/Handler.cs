using ECC.DanceCup.Api.Application.Abstractions.Caching;
using ECC.DanceCup.Api.Application.Abstractions.Metrics;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetDances;

public static partial class GetDancesUseCase
{
    public class QueryHandler(
        IDanceViewCache danceViewCache,
        IDanceViewRepository danceViewRepository,
        IMetrics metrics
    ) : IRequestHandler<Query, Result<QueryResponse>>
    {
        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var dances = await danceViewCache.FindAllAsync(cancellationToken);
            if (dances is null)
            {
                dances = await danceViewRepository.FindAllAsync(cancellationToken);
                await danceViewCache.InsertRangeAsync(dances, cancellationToken);
                metrics.IncDanceCacheMiss();
            }
            else
            {
                metrics.IncDanceCacheHit();
            }

            return new QueryResponse(dances);
        }
    }
}