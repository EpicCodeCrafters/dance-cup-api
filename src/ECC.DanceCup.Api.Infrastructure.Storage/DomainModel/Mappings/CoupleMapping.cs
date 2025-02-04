using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class CoupleMapping
{
    public static CoupleDbo ToDbo(this Couple couple)
    {
        return new CoupleDbo
        {
            Id = couple.Id.Value,
            TournamentId = couple.TournamentId.Value,
            FirstParticipantFullName = couple.FirstParticipantFullName.Value,
            SecondParticipantFullName = couple.SecondParticipantFullName?.Value,
            DanceOrganizationName = couple.DanceOrganizationName?.Value,
            FirstTrainerFullName = couple.FirstTrainerFullName?.Value,
            SecondTrainerFullName = couple.SecondTrainerFullName?.Value
        };
    }
    
    public static List<Couple> ToDomain(this IEnumerable<CoupleDbo> couples)
    {
        return couples.Select(c => 
            new Couple(
                id: CoupleId.From(c.Id).AsRequired(),
                tournamentId: TournamentId.From(c.TournamentId).AsRequired(),
                firstParticipantFullName: CoupleParticipantFullName.From(c.FirstParticipantFullName).AsRequired(),
                secondParticipantFullName: c.SecondParticipantFullName != null ? CoupleParticipantFullName.From(c.SecondParticipantFullName) : null,
                danceOrganizationName: c.DanceOrganizationName != null ? CoupleDanceOrganizationName.From(c.DanceOrganizationName) : null,
                firstTrainerFullName: c.FirstTrainerFullName != null ? CoupleTrainerFullName.From(c.FirstTrainerFullName) : null,
                secondTrainerFullName: c.SecondTrainerFullName != null ? CoupleTrainerFullName.From(c.SecondTrainerFullName) : null
            )).ToList().AsRequired();
    }
}