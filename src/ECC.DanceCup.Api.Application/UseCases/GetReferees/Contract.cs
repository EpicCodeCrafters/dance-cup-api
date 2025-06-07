using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetReferees;

public static partial class GetRefereesUseCase
{
    public record Query(RefereeFullName? RefereeFullName, int PageNumber, int PageSize) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<RefereeView> Referees);
}