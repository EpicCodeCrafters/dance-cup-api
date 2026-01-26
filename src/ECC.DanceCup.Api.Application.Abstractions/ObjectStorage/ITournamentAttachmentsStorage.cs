using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.ObjectStorage;

public interface ITournamentAttachmentsStorage
{
    Task PutAttachmentAsync(
        TournamentId tournamentId,
        int attachmentNumber,
        IAsyncEnumerable<byte[]> attachmentBytes, 
        CancellationToken cancellationToken);

    Task DeleteAttachment(
        TournamentId tournamentId,
        int attachmentNumber,
        CancellationToken cancellationToken);
}