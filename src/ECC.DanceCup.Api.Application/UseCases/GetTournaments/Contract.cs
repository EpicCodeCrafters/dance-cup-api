using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournaments;

public static partial class GetTournamentsUseCase
{
    public record Query(
        UserId UserId,
        int PageNumber,
        int PageSize
    ) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<TournamentView> Tournaments);
}