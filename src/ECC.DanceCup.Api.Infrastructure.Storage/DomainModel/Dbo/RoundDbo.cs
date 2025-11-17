namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class RoundDbo
{
    public long Id { get; set; }
    
    public required long CategoryId { get; set; }
    
    public required int OrderNumber { get; set; }
}
