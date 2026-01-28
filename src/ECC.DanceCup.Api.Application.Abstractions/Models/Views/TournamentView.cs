using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Models.Views;

public class TournamentView
{
    public long Id { get; set; }
    
    public required long UserId { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public DateTime Date { get; set; }
    
    public required string State { get; set; }
    
    public List<CategoryView> Categories { get; set; } = new List<CategoryView>();
}