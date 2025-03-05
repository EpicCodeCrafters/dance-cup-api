namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

public class UserDbo
{
    public long Id { get; set; }
    
    public int Version { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    public long ExternalId { get; set; }

    public string Username { get; set; } = string.Empty;
}