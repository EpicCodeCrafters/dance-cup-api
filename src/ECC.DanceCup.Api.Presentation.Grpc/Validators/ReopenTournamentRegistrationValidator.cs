﻿using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Validators;

public class ReopenTournamentRegistrationValidator : AbstractValidator<ReopenTournamentRegistrationRequest>
{
    public ReopenTournamentRegistrationValidator()
    {
        RuleFor(request => request.TournamentId).IsValidTournamentId();
    }
}