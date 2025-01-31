using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Services;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateTournament;

public static partial class CreateTournamentUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result<CommandResponse>>
    {
        private readonly ITournamentFactory _tournamentFactory;
        private readonly ITournamentRepository _tournamentRepository;

        public CommandHandler(ITournamentFactory tournamentFactory, ITournamentRepository tournamentRepository)
        {
            _tournamentFactory = tournamentFactory;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Result<CommandResponse>> Handle(Command command, CancellationToken cancellationToken)
        {
            var createTournamentResult = _tournamentFactory.Create(command.UserId, command.Name, command.Description, command.Date, command.CreateCategoryModels);
            if (createTournamentResult.IsFailed)
            {
                return createTournamentResult.ToResult();
            }

            var tournament = createTournamentResult.Value;
            var tournamentId = await _tournamentRepository.InsertAsync(tournament, cancellationToken);

            return new CommandResponse(tournamentId);
        }
    }
}