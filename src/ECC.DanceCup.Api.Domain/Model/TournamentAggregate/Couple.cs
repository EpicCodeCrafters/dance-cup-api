using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

public class Couple :  Entity<CoupleId>
{
    public Couple(
        CoupleId id,
        TournamentId tournamentId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName
    ) : base(id)
    {
        TournamentId = tournamentId;
        FirstParticipantFullName = firstParticipantFullName;
        SecondParticipantFullName = secondParticipantFullName;
        DanceOrganizationName = danceOrganizationName;
        FirstTrainerFullName = firstTrainerFullName;
        SecondTrainerFullName = secondTrainerFullName;
    }
    
    public TournamentId TournamentId { get; }
    
    public CoupleParticipantFullName FirstParticipantFullName { get; }
    
    public CoupleParticipantFullName? SecondParticipantFullName { get; }
    
    public CoupleDanceOrganizationName? DanceOrganizationName { get; }
    
    public CoupleTrainerFullName? FirstTrainerFullName { get; }
    
    public CoupleTrainerFullName? SecondTrainerFullName { get; }
}