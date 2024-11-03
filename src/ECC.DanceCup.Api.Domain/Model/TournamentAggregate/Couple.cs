using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Танцевальная пара
/// </summary>
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
    
    /// <summary>
    /// Идентификатор турнира, в котором участвует пара
    /// </summary>
    public TournamentId TournamentId { get; }
    
    /// <summary>
    /// Полное имя первого участника пары
    /// </summary>
    public CoupleParticipantFullName FirstParticipantFullName { get; }
    
    /// <summary>
    /// Второе имя участника пары
    /// </summary>
    public CoupleParticipantFullName? SecondParticipantFullName { get; }
    
    /// <summary>
    /// Название организации, в которой пара занимается танцами
    /// </summary>
    public CoupleDanceOrganizationName? DanceOrganizationName { get; }
    
    /// <summary>
    /// Полное имя первого тренера пары
    /// </summary>
    public CoupleTrainerFullName? FirstTrainerFullName { get; }
    
    /// <summary>
    /// Полное имя второго тренера пары
    /// </summary>
    public CoupleTrainerFullName? SecondTrainerFullName { get; }
}