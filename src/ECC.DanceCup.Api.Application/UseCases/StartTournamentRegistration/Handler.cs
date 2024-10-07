using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.StartTournamentRegistration;

public static partial class StartTournamentRegistrationUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly ITournamentRepository _tournamentRepository;

        public CommandHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindAsync(command.TournamentId, cancellationToken);
            if (tournament is null)
            {
                return new TournamentNotFoundError(command.TournamentId);
            }

            var startTournamentRegistrationResult = tournament.StartRegistration();
            if (startTournamentRegistrationResult.IsFailed)
            {
                return startTournamentRegistrationResult;
            }

            await _tournamentRepository.UpdateAsync(tournament, cancellationToken);
            
            return Result.Ok();
        }
    }
}