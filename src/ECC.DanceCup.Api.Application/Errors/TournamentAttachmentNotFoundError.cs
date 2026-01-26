using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Errors;

public class TournamentAttachmentNotFoundError : NotFoundError
{
    public TournamentAttachmentNotFoundError(TournamentId tournamentId, int attachmentNumber)
        : base($"Не найдено прикрипление номер {attachmentNumber} турнира {tournamentId.Value}")
    {
    }
}