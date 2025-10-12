namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class CoupleDbo
{
    public long Id { get; set; }
    
    public required long TournamentId { get; set; }
    
    public required string FirstParticipantFullName { get; set; }

    public string? SecondParticipantFullName { get; set; }
    
    public string? DanceOrganizationName { get; set; }
    
    public string? Division { get; set; }
    
    public string? FirstTrainerFullName { get; set; }
    
    public string? SecondTrainerFullName { get; set; }
}