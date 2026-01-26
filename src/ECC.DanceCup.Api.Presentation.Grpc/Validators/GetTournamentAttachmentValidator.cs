using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class GetTournamentAttachmentValidator : AbstractValidator<GetTournamentAttachmentRequest>
{
    public GetTournamentAttachmentValidator()
    {
        RuleFor(request => request.TournamentId)
            .IsValidTournamentId();

        RuleFor(request => request.AttachmentNumber)
            .GreaterThan(0)
            .WithMessage("Номер прикрепления должен быть положительным");
        
        RuleFor(request => request.MaxBytesCount)
            .GreaterThan(0)
            .WithMessage("Максимальное количество байт в батче должно быть положительным");
    }
}