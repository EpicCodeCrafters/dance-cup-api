using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.TgApi;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.FinishTournamentRegistration;

public static partial class FinishTournamentRegistrationUseCase
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

            var finishTournamentRegistrationResult = tournament.FinishRegistration();
            if (finishTournamentRegistrationResult.IsFailed)
            {
                return finishTournamentRegistrationResult;
            }

            await _tournamentRepository.UpdateAsync(tournament, cancellationToken);

            await _tgApi.SendMessageAsync(
                $"✅ Регистрация на турнир \"{tournament.Name}\" завершена!\n\n" +
                $"👥 Зарегистрировано пар: {tournament.CouplesCount}\n" +
                $"📋 Категорий: {tournament.CategoriesCount}",
                cancellationToken
            );

            return Result.Ok();
        }
    }
}