using AutoFixture;
using ECC.DanceCup.Api.Domain.Model;

namespace ECC.DanceCup.Api.Tests.Common.Extensions;

public static class FixtureExtensions
{
    public static Category CreateCategory(
        this IFixture fixture,
        CategoryId? id = null,
        TournamentId? tournamentId = null,
        CategoryName? name = null,
        List<DanceId>? dancesIds = null,
        List<RefereeId>? refereesIds = null)
    {
        id ??= fixture.Create<CategoryId>();
        tournamentId ??= fixture.Create<TournamentId>();
        name ??= fixture.Create<CategoryName>();
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
    
    public static Tournament CreateTournament(
        this IFixture fixture,
        TournamentId? id = null, 
        int? version = null,
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
        version ??= fixture.Create<int>();
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
}