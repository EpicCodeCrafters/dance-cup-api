using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentCategories;

public static partial class GetTournamentCategoriesUseCase
{
    public record Query(
        TournamentId TournamentId
    ) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<CategoryView> Categories);
}
