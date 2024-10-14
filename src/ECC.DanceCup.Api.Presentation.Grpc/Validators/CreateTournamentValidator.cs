using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class CreateTournamentValidator : AbstractValidator<CreateTournamentRequest>
{
    public CreateTournamentValidator()
    {
        RuleFor(request => request.UserId).IsValidUserId();

        RuleFor(request => request.Name).IsValidTournamentName();

        RuleFor(request => request.Description).IsValidTournamentDescription();

        RuleFor(request => request.Date).IsValidTournamentDate();

        RuleForEach(request => request.CreateCategoryModels).SetValidator(new CreateCategoryModelValidator());
    }
}

public class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryModelValidator()
    {
        RuleFor(model => model.Name).IsValidCategoryName();

        RuleFor(model => model.DancesIds)
            .NotEmpty()
            .WithMessage("Необходимо передать список идентификаторов танцев категории");

        RuleForEach(model => model.DancesIds).IsValidDanceId();
        
        RuleFor(model => model.RefereesIds)
            .NotEmpty()
            .WithMessage("Необходимо передать список идентификаторов судей категории");

        RuleForEach(model => model.RefereesIds).IsValidRefereeId();
    }
}