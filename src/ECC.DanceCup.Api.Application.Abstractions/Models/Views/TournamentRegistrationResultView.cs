namespace ECC.DanceCup.Api.Application.Abstractions.Models.Views;

public class TournamentRegistrationResultView
{
    public required string CategoryName { get; init; }
    
    public required string FirstParticipantFullName { get; init; }
    
    public required string? SecondParticipantFullName { get; init; }
    
    public required string? DanceOrganizationName { get; init; }
    
    public required string? Division { get; init; }
    
    public required string? FirstTrainerFullName { get; init; }
    
    public required string? SecondTrainerFullName { get; init; }
}