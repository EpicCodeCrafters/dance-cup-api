using ECC.DanceCup.Api.Application.Abstractions.ObjectStorage;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentAttachment;

public static partial class GetTournamentAttachmentUseCase
{
    public class QueryHandler(
        ITournamentViewRepository tournamentViewRepository,
        ITournamentAttachmentsStorage tournamentAttachmentsStorage
    ) : IRequestHandler<Query, Result<QueryResponse>>
    {
        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var attachmentName = await tournamentViewRepository.GetTournamentAttachmentNameAsync(
                query.TournamentId,
                query.AttachmentNumber,
                cancellationToken
            );
            if (attachmentName is null)
            {
                return new TournamentAttachmentNotFoundError(query.TournamentId, query.AttachmentNumber);
            }

            var totalAttachmentBytesCount = await tournamentAttachmentsStorage.GetTotalAttachmentBytesCount(
                query.TournamentId,
                query.AttachmentNumber,
                cancellationToken
            );

            var attachmentBytes = tournamentAttachmentsStorage.GetAttachmentAsync(
                query.TournamentId,
                query.AttachmentNumber,
                query.MaxBytesCount,
                cancellationToken
            );
            
            return new QueryResponse(attachmentName, totalAttachmentBytesCount, attachmentBytes);
        }
    }
}