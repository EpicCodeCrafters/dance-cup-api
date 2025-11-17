using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetRoundsByCategory;

public static partial class GetRoundsByCategoryUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly ITournamentRepository _tournamentRepository;

        public QueryHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindByIdAsync(query.TournamentId, cancellationToken);
            
            if (tournament == null)
            {
                return Result.Fail("Tournament not found");
            }

            var category = tournament.Categories.FirstOrDefault(c => c.Id == query.CategoryId);

            if (category == null)
            {
                return Result.Fail("Category not found");
            }

            var rounds = category.Rounds
                .Select(r => new RoundDto(
                    Id: r.Id.Value,
                    OrderNumber: r.OrderNumber.Value,
                    CoupleIds: r.CouplesIds.Select(c => c.Value).ToList()
                ))
                .ToList();

            return new QueryResponse(rounds);
        }
    }
}
