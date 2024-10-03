using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateTournament;

public static partial class CreateTournamentUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result<CommandResponse>>
    {
        public async Task<Result<CommandResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            return Result.Fail("Пока не сделано");
        }
    }
}