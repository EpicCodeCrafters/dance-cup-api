using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class ListTournamentAttachmentsValidator : AbstractValidator<ListTournamentAttachmentsRequest>
{
    public ListTournamentAttachmentsValidator()
    {
        RuleFor(request => request.TournamentId)
            .IsValidTournamentId();
    }
}