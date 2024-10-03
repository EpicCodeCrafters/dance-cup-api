using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetDances;

public static partial class GetDancesUseCase
{
    public record Query : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<DanceView> Dances);
}