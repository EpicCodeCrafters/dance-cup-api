﻿using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.StartTournamentRegistration;

public static partial class StartTournamentRegistrationUseCase
{
    public record Command(TournamentId TournamentId) : IRequest<Result>;
}