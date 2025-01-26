namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class CategoryDbo
{
    public long Id { get; set; }
    
    public required long TournamentId { get; set; }
    
    public required string Name{ get; set; }
}