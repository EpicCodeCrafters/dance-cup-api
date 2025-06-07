using ECC.DanceCup.Api.Application.Abstractions.Caching;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetDances;

public static partial class GetDancesUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly IDanceViewCache _danceViewCache;
        private readonly IDanceViewRepository _danceViewRepository;

        public QueryHandler(
            IDanceViewCache danceViewCache, 
            IDanceViewRepository danceViewRepository)
        {
            _danceViewCache = danceViewCache;
            _danceViewRepository = danceViewRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var dances = await _danceViewCache.FindAllAsync(cancellationToken);
            if (dances is null)
            {
                dances = await _danceViewRepository.FindAllAsync(cancellationToken);
                await _danceViewCache.InsertRangeAsync(dances, cancellationToken);
            }

            return new QueryResponse(dances);
        }
    }
}