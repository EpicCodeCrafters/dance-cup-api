namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class TournamentDbo
{
    public long Id { get; set; }
    
    public int Version { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    public long UserId { get; set; }
    
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    
    public required string State { get; set; }
    
    public DateTime? RegistrationStartedAt { get; set; }
    
    public DateTime? RegistrationFinishedAt { get; set; }
    
    public DateTime? StartedAt { get; set; }
    
    public DateTime? FinishedAt { get; set; }
    
    public string Attachments { get; set; } = string.Empty;
}