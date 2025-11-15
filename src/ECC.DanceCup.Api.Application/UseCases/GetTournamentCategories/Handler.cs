using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentCategories;

public static partial class GetTournamentCategoriesUseCase
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
            var categories = await _tournamentViewRepository.GetCategoriesAsync(
                request.TournamentId,
                cancellationToken
            );

            return new QueryResponse(categories);
        }
    }
}
