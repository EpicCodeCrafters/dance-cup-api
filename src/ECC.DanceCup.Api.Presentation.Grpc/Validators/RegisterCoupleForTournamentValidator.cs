using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class RegisterCoupleForTournamentValidator : AbstractValidator<RegisterCoupleForTournamentRequest>
{
    public RegisterCoupleForTournamentValidator()
    {
        RuleFor(request => request.TournamentId).IsValidTournamentId();

        RuleFor(request => request.FirstParticipantFullName).IsValidCoupleParticipantFullName();
        
        RuleFor(request => request.SecondParticipantFullName)
            .IsValidCoupleParticipantFullName()
            .UnlessNull();
        
        RuleFor(request => request.DanceOrganizationName)
            .IsValidCoupleDanceOrganizationName()
            .UnlessNull();

        RuleFor(request => request.FirstTrainerFullName)
            .IsValidCoupleTrainerFullName()
            .UnlessNull();
        
        RuleFor(request => request.SecondTrainerFullName)
            .IsValidCoupleTrainerFullName()
            .UnlessNull();

        RuleFor(request => request.CategoriesIds)
            .NotEmpty()
            .WithMessage("Необходимо передать список идентификаторов категорий");

        RuleForEach(request => request.CategoriesIds).IsValidCategoryId();
    }
}