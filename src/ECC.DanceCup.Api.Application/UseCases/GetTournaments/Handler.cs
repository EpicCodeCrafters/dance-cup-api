using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournaments;

public static partial class GetTournamentsUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly ITournamentViewRepository _tournamentViewRepository;

        public QueryHandler(ITournamentViewRepository tournamentViewRepository)
        {
            _tournamentViewRepository = tournamentViewRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentViewRepository.FindAllAsync(
                request.UserId,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            return new QueryResponse(tournaments);
        }
    }
}