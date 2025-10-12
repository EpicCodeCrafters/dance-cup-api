using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.ReopenTournamentRegistration;

public static partial class ReopenTournamentRegistrationUseCase
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

            var reopenTournamentRegistrationResult = tournament.ReopenRegistration();
            if (reopenTournamentRegistrationResult.IsFailed)
            {
                return reopenTournamentRegistrationResult;
            }

            await _tournamentRepository.UpdateAsync(tournament, cancellationToken);

            await _tgApi.SendMessageAsync(
                $"🔄 Регистрация на турнир \"{tournament.Name}\" возобновлена!\n\n" +
                $"📅 Дата турнира: {tournament.Date}\n" +
                $"👥 Уже зарегистрировано пар: {tournament.CouplesCount}",
                cancellationToken
            );

            return Result.Ok();
        }
    }
}