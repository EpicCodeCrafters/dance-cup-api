using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class FinishTournamentRegistrationValidator : AbstractValidator<FinishTournamentRegistrationRequest>
{
    public FinishTournamentRegistrationValidator()
    {
        RuleFor(request => request.TournamentId).IsValidTournamentId();
    }
}