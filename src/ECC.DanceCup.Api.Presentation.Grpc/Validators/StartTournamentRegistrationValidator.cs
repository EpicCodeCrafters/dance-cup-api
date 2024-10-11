using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class StartTournamentRegistrationValidator : AbstractValidator<StartTournamentRegistrationRequest>
{
    public StartTournamentRegistrationValidator()
    {
        RuleFor(request => request.TournamentId).IsValidTournamentId();
    }
}