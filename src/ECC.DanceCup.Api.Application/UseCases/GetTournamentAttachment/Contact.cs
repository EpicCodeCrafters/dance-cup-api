using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentAttachment;

public static partial class GetTournamentAttachmentUseCase
{
    public record Query(
        TournamentId TournamentId,
        int AttachmentNumber,
        int MaxBytesCount
    ) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(
        string AttachmentName,
        long TotalAttachmentBytesCount,
        IAsyncEnumerable<byte[]> AttachmentBytes
    );
}