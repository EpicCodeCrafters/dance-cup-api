using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class GetTournamentsValidator : AbstractValidator<GetTournamentsRequest>
{
    public GetTournamentsValidator()
    {
        RuleFor(request => request.UserId).IsValidUserId();

        RuleFor(request => request.PageNumber).IsValidPageNumber();

        RuleFor(request => request.PageSize).IsValidPageSize();
    }
}