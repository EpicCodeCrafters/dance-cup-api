using AutoFixture;
using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;

namespace ECC.DanceCup.Api.Tests.Common.Extensions;

public static class FixtureExtensions
{
    public static Referee CreateReferee(
        this IFixture fixture,
        RefereeId? id = null, 
        AggregateVersion? version = null,
        DateTime? createdAt = null, 
        DateTime? changedAt = null,
        RefereeFullName? fullName = null)
    {
        id ??= fixture.Create<RefereeId>();
        version ??= fixture.Create<AggregateVersion>();
        createdAt ??= fixture.Create<DateTime>();
        changedAt ??= fixture.Create<DateTime>();
        fullName ??= fixture.Create<RefereeFullName>();

        return new Referee(
            id: id.Value,
            version: version.Value,
            createdAt: createdAt.Value,
            changedAt: changedAt.Value,
            fullName: fullName.Value
        );
    }
    
    public static Tournament CreateTournament(
        this IFixture fixture,
        TournamentId? id = null, 
        AggregateVersion? version = null,
        DateTime? createdAt = null, 
        DateTime? changedAt = null,
        UserId? userId = null,
        TournamentName? name = null,
        TournamentDate? date = null,
        TournamentState? state = null,
        DateTime? registrationStartedAt = null,
        DateTime? registrationFinishedAt = null,
        DateTime? startedAt = null,
        DateTime? finishedAt = null,
        List<Category>? categories = null)
    {
        id ??= fixture.Create<TournamentId>();
        version ??= fixture.Create<AggregateVersion>();
        createdAt ??= fixture.Create<DateTime>();
        changedAt ??= fixture.Create<DateTime>();
        userId ??= fixture.Create<UserId>();
        name ??= fixture.Create<TournamentName>();
        date ??= fixture.Create<TournamentDate>();
        state ??= fixture.Create<TournamentState>();
        categories ??= fixture.Create<List<Category>>();
        
        return new Tournament(
            id: id.Value,
            version: version.Value,
            createdAt: createdAt.Value,
            changedAt: changedAt.Value,
            userId: userId.Value,
            name: name.Value,
            date: date.Value,
            state: state.Value,
            registrationStartedAt: registrationStartedAt,
            registrationFinishedAt: registrationFinishedAt,
            startedAt: startedAt,
            finishedAt: finishedAt,
            categories: categories
        );
    }
    
    public static Category CreateCategory(
        this IFixture fixture,
        CategoryId? id = null,
        TournamentId? tournamentId = null,
        TournamentDescription? name = null,
        List<DanceId>? dancesIds = null,
        List<RefereeId>? refereesIds = null)
    {
        id ??= fixture.Create<CategoryId>();
        tournamentId ??= fixture.Create<TournamentId>();
        name ??= fixture.Create<TournamentDescription>();
        dancesIds ??= fixture.Create<List<DanceId>>();
        refereesIds ??= fixture.Create<List<RefereeId>>();
        
        return new Category(
            id: id.Value,
            tournamentId: tournamentId.Value,
            name: name.Value,
            dancesIds: dancesIds,
            refereesIds: refereesIds
        );
    }
}