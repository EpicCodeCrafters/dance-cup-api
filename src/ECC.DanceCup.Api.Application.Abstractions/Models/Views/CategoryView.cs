namespace ECC.DanceCup.Api.Application.Abstractions.Models.Views;

public class CategoryView
{
    public required long Id { get; set; }
    
    public required string Name { get; set; }
    
    public required long TournamentId { get; set; }
}