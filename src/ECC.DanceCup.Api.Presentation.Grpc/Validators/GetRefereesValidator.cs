using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class GetRefereesValidator : AbstractValidator<GetRefereesRequest>
{
    public GetRefereesValidator()
    {
        RuleFor(request => request.FullName).IsValidRefereeFullName().Unless(value => value is null);

        RuleFor(request => request.PageNumber).IsValidPageNumber();

        RuleFor(request => request.PageSize).IsValidPageSize();
    }
}