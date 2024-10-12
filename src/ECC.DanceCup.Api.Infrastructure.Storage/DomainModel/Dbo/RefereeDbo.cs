namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class RefereeDbo
{ 
    public long Id { get; set; }
    
    public int Version { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ChangedAt { get; set; }

    public string FullName { get; set; } = string.Empty;
}