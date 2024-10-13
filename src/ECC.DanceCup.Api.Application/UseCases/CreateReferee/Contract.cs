using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateReferee;

public static partial class CreateRefereeUseCase
{
    public record Command(RefereeFullName FullName) : IRequest<Result<CommandResponse>>;

    public record CommandResponse(RefereeId RefereeId);
}