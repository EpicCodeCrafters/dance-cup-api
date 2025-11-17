namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;

internal class RoundCoupleDbo
{
    public long Id { get; set; }
    
    public required long RoundId { get; set; }
    
    public required long CoupleId { get; set; }
}
