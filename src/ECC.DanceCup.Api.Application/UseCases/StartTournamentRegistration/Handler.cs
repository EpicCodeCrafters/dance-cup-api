using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.StartTournamentRegistration;

public static partial class StartTournamentRegistrationUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITgApi _tgApi;

        public CommandHandler(ITournamentRepository tournamentRepository, ITgApi tgApi)
        {
            _tournamentRepository = tournamentRepository;
            _tgApi = tgApi;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindByIdAsync(command.TournamentId, cancellationToken);
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
            
            await _tgApi.SendMessageAsync(
                $"🎉 Регистрация на турнир \"{tournament.Name}\" началась!\n\n" +
                $"📅 Дата турнира: {tournament.Date}\n" +
                $"📝 Описание: {tournament.Description}",
                cancellationToken
            );
            
            return Result.Ok();
        }
    }
}