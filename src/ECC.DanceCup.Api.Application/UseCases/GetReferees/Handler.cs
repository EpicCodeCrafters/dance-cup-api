using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetReferees;

public static partial class GetRefereesUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly IRefereeViewRepository _refereeViewRepository;

        public QueryHandler(IRefereeViewRepository refereeViewRepository)
        {
            _refereeViewRepository = refereeViewRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var referees = await _refereeViewRepository.FindAllAsync(query.RefereeFullName, query.pageNumber,query.pageSize, cancellationToken);

            return new QueryResponse(referees);
        }
    }
}