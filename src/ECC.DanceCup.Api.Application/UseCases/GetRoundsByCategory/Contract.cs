using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetRoundsByCategory;

public static partial class GetRoundsByCategoryUseCase
{
    public record Query(TournamentId TournamentId, CategoryId CategoryId) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<RoundDto> Rounds);

    public record RoundDto(
        long Id,
        int OrderNumber,
        IReadOnlyCollection<long> CoupleIds
    );
}
