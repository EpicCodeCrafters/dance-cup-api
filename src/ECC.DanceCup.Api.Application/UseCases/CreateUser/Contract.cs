using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateUser;

public static partial class CreateUserUseCase
{
    public record Command(
        UserExternalId ExternalId,
        Username Username
    ) : IRequest<Result<CommandResponse>>;

    public record CommandResponse(UserId UserId);
}