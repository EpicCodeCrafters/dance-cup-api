using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Domain.Services;
using FluentResults;
using MediatR;
using System.Collections.Generic;
using System.Xml.Linq;

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
            // TODO Передавать какие-то данные
            var createTournamentResult = _tournamentFactory.Create(command.userId,command.name, command.catigories);
            if (createTournamentResult.IsFailed)
            {
                return createTournamentResult.ToResult();
            }

            var tournament = createTournamentResult.Value;
            var tournamentId = await _tournamentRepository.AddAsync(tournament, cancellationToken);

            return new CommandResponse(tournamentId);
        }
    }
}