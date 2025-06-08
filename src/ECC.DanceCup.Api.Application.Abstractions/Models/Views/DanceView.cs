namespace ECC.DanceCup.Api.Application.Abstractions.Models.Views;

public class DanceView
{
    public required long Id { get; init; }
    
    public required string ShortName { get; init; }
    
    public required string Name { get; init; }
}