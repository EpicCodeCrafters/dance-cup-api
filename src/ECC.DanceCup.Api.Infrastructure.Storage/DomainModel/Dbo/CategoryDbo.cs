namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class CategoryDbo
{
    public long Id { get; set; }
    
    public long TournamentId { get; set; }
    
    public string Name { get; set; } = string.Empty;
}