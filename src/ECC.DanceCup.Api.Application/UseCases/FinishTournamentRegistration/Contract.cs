using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.FinishTournamentRegistration;

public static partial class FinishTournamentRegistrationUseCase
{
    public record Command(TournamentId TournamentId) : IRequest<Result>;
}