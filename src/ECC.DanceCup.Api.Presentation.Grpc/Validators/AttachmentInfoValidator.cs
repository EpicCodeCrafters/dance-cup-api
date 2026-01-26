using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class AttachmentInfoValidator : AbstractValidator<AddTournamentAttachmentRequest.Types.AttachmentInfo>
{
    public AttachmentInfoValidator()
    {
        RuleFor(request => request.TournamentId)
            .IsValidTournamentId();

        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Название файла не должно быть пустым");
    }
}