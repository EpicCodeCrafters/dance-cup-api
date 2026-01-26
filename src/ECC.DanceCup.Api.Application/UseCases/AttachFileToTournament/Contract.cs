using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.AttachFileToTournament;

public static partial class AttachFileToTournamentUseCase
{
    public record Command(
        TournamentId TournamentId,
        string AttachmentName,
        IAsyncEnumerable<byte[]> AttachmentBytes
    ) : IRequest<Result>;
}