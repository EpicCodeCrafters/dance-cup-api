﻿using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;

public class TournamentRegistrationResultView
{
    public required CategoryName CategoryName { get; init; }
    
    public required string FirstParticipantFullName { get; init; }
    
    public required string? SecondParticipantFullName { get; init; }
    
    public required string? DanceOrganizationName { get; init; }
    
    public required string? FirstTrainerFullName { get; init; }
    
    public required string? SecondTrainerFullName { get; init; }
}