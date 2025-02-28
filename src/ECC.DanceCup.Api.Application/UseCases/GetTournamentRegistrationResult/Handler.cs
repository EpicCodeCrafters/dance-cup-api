using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentRegistrationResult;

public static partial class GetTournamentRegistrationResultUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        public readonly ITournamentViewRepository _repository;

        public QueryHandler(ITournamentViewRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var resultOfRegistration = await _repository.GetRegistrationResultAsync(query.TournamentId, cancellationToken);
            
            return new QueryResponse(resultOfRegistration);
        }
    }
}