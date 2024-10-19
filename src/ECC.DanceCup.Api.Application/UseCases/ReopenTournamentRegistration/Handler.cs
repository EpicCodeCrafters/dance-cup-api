using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.ReopenTournamentRegistration;

public static partial class ReopenTournamentRegistrationUseCase
{
    public class CommandHandler
    {
        private readonly ITournamentRepository _tournamentRepository;

        public CommandHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Result> Handle(Command command,CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindAsync(command.TournamentId, cancellationToken);
            if (tournament == null)
            {
                return new TournamentNotFoundError(command.TournamentId);
            }

            var reopenResult = tournament.ReopenRegistration();
            if (reopenResult.IsFailed)
            {
                return reopenResult;
            }

            await _tournamentRepository.UpdateAsync(tournament, cancellationToken);

            return Result.Ok();
        }
    }
}