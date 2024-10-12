using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class CreateRefereeValidator : AbstractValidator<CreateRefereeRequest>
{
    public CreateRefereeValidator()
    {
        RuleFor(request => request.FullName).IsValidRefereeFullName();
    }
}