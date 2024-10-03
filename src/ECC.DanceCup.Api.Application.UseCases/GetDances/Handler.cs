using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetDances;

public static partial class GetDancesUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly IDanceViewRepository _danceViewRepository;

        public QueryHandler(IDanceViewRepository danceViewRepository)
        {
            _danceViewRepository = danceViewRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var dances = await _danceViewRepository.FindAllAsync(cancellationToken);

            return new QueryResponse(dances);
        }
    }
}