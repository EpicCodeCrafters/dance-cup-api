using ECC.DanceCup.Api.Application.Abstractions.ObjectStorage;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.AttachFileToTournament;

public static partial class AttachFileToTournamentUseCase
{
    public class CommandHandler(
        ITournamentRepository tournamentRepository,
        ITournamentAttachmentsStorage tournamentAttachmentsStorage
    ) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tournament = await tournamentRepository.FindByIdAsync(command.TournamentId, cancellationToken);
            if (tournament is null)
            {
                return new TournamentNotFoundError(command.TournamentId);
            }

            var addTournamentAttachmentResult = tournament.AddAttachment(command.AttachmentName);
            if (addTournamentAttachmentResult.IsFailed)
            {
                return addTournamentAttachmentResult.ToResult();
            }
            var attachment = addTournamentAttachmentResult.Value;

            await tournamentAttachmentsStorage.PutAttachmentAsync(
                tournament.Id,
                attachment.Number,
                command.AttachmentBytes, 
                cancellationToken
            );

            try
            {
                await tournamentRepository.UpdateAsync(tournament, cancellationToken);
            }
            catch
            {
                await tournamentAttachmentsStorage.DeleteAttachment(
                    tournament.Id,
                    attachment.Number,
                    cancellationToken
                );
                throw;
            }

            return Result.Ok();
        }
    }
}