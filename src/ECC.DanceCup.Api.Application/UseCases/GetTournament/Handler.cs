using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournament;

public static partial class GetTournamentUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly ITournamentViewRepository _tournamentRepository;

        public QueryHandler(ITournamentViewRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindOneAsync(request.TournamentId, cancellationToken);

            return new QueryResponse(tournament);
        }
    }
}