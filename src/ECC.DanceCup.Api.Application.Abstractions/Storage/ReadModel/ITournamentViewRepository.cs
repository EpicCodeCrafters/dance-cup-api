﻿using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface ITournamentViewRepository
{
    Task<IReadOnlyCollection<TournamentView>> FindAllAsync(UserId userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<TournamentRegistrationResultView>> GetRegistrationResultAsync(TournamentId tournamentId, CancellationToken cancellationToken);
}