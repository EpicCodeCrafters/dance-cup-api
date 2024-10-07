using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class StartTournamentRegistrationValidator : AbstractValidator<StartTournamentRegistrationRequest>
{
    public StartTournamentRegistrationValidator()
    {
        RuleFor(request => request.TournamentId)
            .GreaterThan(0)
            .WithMessage("Необходимо передать корректный идентификатор турнира");
    }
}