using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.RegisterCoupleForTournament;

public static partial class RegisterCoupleForTournamentUseCase
{
    public record Command(
        TournamentId TournamentId,
        CoupleParticipantFullName FirstParticipantFullName,
        CoupleParticipantFullName? SecondParticipantFullName, 
        CoupleDanceOrganizationName? DanceOrganizationName, 
        CoupleTrainerFullName? FirstTrainerFullName, 
        CoupleTrainerFullName? SecondTrainerFullName,
        IReadOnlyCollection<CategoryId> CategoriesIds
    ) : IRequest<Result>;
}