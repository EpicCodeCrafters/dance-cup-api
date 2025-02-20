using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class GetTournamentRegistrationResultValidator : AbstractValidator<GetTournamentRegistrationResultRequest>
{
    public GetTournamentRegistrationResultValidator()
    {
        RuleFor(request => request.TournamentId).IsValidTournamentId();
    }
}