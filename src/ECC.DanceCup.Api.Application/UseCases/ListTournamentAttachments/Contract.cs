using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.ListTournamentAttachments;

public static partial class ListTournamentAttachmentsUseCase
{
    public record Query(
        TournamentId TournamentId
    ) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(IReadOnlyCollection<TournamentAttachmentView> Attachments);
}