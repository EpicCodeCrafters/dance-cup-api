using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.Providers;
using ECC.DanceCup.Api.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.RegisterCoupleForTournament;

public static partial class RegisterCoupleForTournamentUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ICoupleIdProvider _coupleIdProvider;

        public CommandHandler(ITournamentRepository tournamentRepository, ICoupleIdProvider coupleIdProvider)
        {
            _tournamentRepository = tournamentRepository;
            _coupleIdProvider = coupleIdProvider;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.FindByIdAsync(command.TournamentId, cancellationToken);
            if (tournament is null)
            {
                return new TournamentNotFoundError(command.TournamentId);
            }

            var coupleId = await _coupleIdProvider.CreateNewAsync(cancellationToken);
            var registerCoupleForTournamentResult = tournament.RegisterCouple(
                coupleId: coupleId,
                firstParticipantFullName: command.FirstParticipantFullName,
                secondParticipantFullName: command.SecondParticipantFullName,
                danceOrganizationName: command.DanceOrganizationName,
                division: command.Division,
                firstTrainerFullName: command.FirstTrainerFullName,
                secondTrainerFullName: command.SecondTrainerFullName,
                categoriesIds: command.CategoriesIds
            );
            if (registerCoupleForTournamentResult.IsFailed)
            {
                return registerCoupleForTournamentResult;
            }

            await _tournamentRepository.UpdateAsync(tournament, cancellationToken);

            return Result.Ok();
        }
    }
}