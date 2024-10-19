using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.ReopenTournamentRegistration;

public static partial class ReopenTournamentRegistrationUseCase
{
    public record Command(TournamentId TournamentId) : IRequest<Result>;
}