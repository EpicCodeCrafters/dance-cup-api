using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.ListTournamentAttachments;

public static partial class ListTournamentAttachmentsUseCase
{
    public class QueryHandler(
        ITournamentViewRepository tournamentViewRepository
    ) : IRequestHandler<Query, Result<QueryResponse>>
    {
        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var attachments = await tournamentViewRepository.GetTournamentAttachmentsAsync(
                query.TournamentId,
                cancellationToken
            );
            if (attachments is null)
            {
                return new TournamentNotFoundError(query.TournamentId);
            }

            return new QueryResponse(attachments);
        }
    }
}