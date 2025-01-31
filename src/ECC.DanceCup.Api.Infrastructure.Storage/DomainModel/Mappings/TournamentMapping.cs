using System.Runtime.CompilerServices;
using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class TournamentMapping
{
    public static TournamentDbo ToDbo(this Tournament tournament)
    {
        return new TournamentDbo
        {
            Id = tournament.Id.Value,
            Version = tournament.Version.Value,
            CreatedAt = tournament.CreatedAt,
            ChangedAt = tournament.ChangedAt,
            UserId = tournament.UserId.Value,
            Name = tournament.Name.Value,
            Description = tournament.Description.Value,
            Date = tournament.Date.Value,
            State = tournament.State.ToString(),
            RegistrationStartedAt = tournament.RegistrationStartedAt,
            RegistrationFinishedAt = tournament.RegistrationFinishedAt,
            StartedAt = tournament.StartedAt,
            FinishedAt = tournament.FinishedAt
        };
    }

    public static CategoryDbo ToDbo(this Category category)
    {
        return new CategoryDbo
        {
            Id = category.Id.Value,
            TournamentId = category.TournamentId.Value,
            Name = category.Name.Value
        };
    }
    public static Tournament ToDomain(this TournamentDbo dbo, IEnumerable<CategoryDbo> _categories, IEnumerable<CoupleDbo> _couples)
    {
        return new Tournament(
            id: TournamentId.From(dbo.Id).AsRequired(),
            version: AggregateVersion.From(dbo.Version).AsRequired(), 
            createdAt: dbo.CreatedAt,
            changedAt: dbo.ChangedAt,
            userId: UserId.From(dbo.UserId).AsRequired(),
            name: TournamentName.From(dbo.Name).AsRequired(),
            description: TournamentDescription.From(dbo.Description).AsRequired(),
            date: TournamentDate.From(dbo.Date).AsRequired(),
            state: Enum.TryParse<TournamentState>(dbo.State, out var state) ? state : throw new ArgumentException("Недопустимое состояние турнира"),
            registrationStartedAt: dbo.RegistrationStartedAt,
            registrationFinishedAt: dbo.RegistrationFinishedAt,
            startedAt: dbo.StartedAt,
            finishedAt: dbo.FinishedAt,
            categories: _categories.Select(c => new Category(
                id: CategoryId.From(c.Id).AsRequired(),
                tournamentId: TournamentId.From(c.TournamentId).AsRequired(),
                name: CategoryName.From(c.Name).AsRequired(),
                dancesIds: new List<DanceId>(), 
                refereesIds: new List<RefereeId>(),
                couplesIds: new List<CoupleId>() 
            )).ToList().AsRequired(),
            couples: _couples.Select(c => new Couple(
                id: CoupleId.From(c.Id).AsRequired(),
                tournamentId: TournamentId.From(c.TournamentId).AsRequired(),
                firstParticipantFullName: CoupleParticipantFullName.From(c.FirstParticipantFullName).AsRequired(),
                secondParticipantFullName: c.SecondParticipantFullName != null ? CoupleParticipantFullName.From(c.SecondParticipantFullName) : null,
                danceOrganizationName: c.DanceOrganizationName != null ? CoupleDanceOrganizationName.From(c.DanceOrganizationName) : null,
                firstTrainerFullName: c.FirstTrainerFullName != null ? CoupleTrainerFullName.From(c.FirstTrainerFullName) : null,
                secondTrainerFullName: c.SecondTrainerFullName != null ? CoupleTrainerFullName.From(c.SecondTrainerFullName) : null
            )).ToList().AsRequired()
        );
    }
}