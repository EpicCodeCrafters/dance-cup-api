using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.RequestsValidators;

public class CreateTournamentValidator : AbstractValidator<CreateTournamentRequest>
{
    public CreateTournamentValidator()
    {
        RuleFor(request => request.UserId)
            .GreaterThan(0)
            .WithMessage("Необходимо передать корректный идентификатор пользователя");

        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Необходимо передать название турнира");

        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage("Необходимо передать дату турнира");

        RuleForEach(request => request.CreateCategoryModels)
            .SetValidator(new CreateCategoryModelValidator());
    }
}

public class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Необходимо передать название категории");

        RuleFor(model => model.DancesIds)
            .NotEmpty()
            .WithMessage("Необходимо передать список идентификаторов танцев категории");

        RuleForEach(model => model.DancesIds)
            .GreaterThan(0)
            .WithMessage("Необходимо передать корректный идентификатор танца");
        
        RuleFor(model => model.RefereesIds)
            .NotEmpty()
            .WithMessage("Необходимо передать список идентификаторов судей категории");

        RuleForEach(model => model.RefereesIds)
            .GreaterThan(0)
            .WithMessage("Необходимо передать корректный идентификатор судьи");
    }
}