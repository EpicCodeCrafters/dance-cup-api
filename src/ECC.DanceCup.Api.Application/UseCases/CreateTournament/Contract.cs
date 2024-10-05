using ECC.DanceCup.Api.Domain.Model;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateTournament;

public static partial class CreateTournamentUseCase
{
    public record Command(UserId userId, TournamentName name, List<Category> catigories) : IRequest<Result<CommandResponse>>;

    public record CommandResponse(TournamentId TournamentId);
}