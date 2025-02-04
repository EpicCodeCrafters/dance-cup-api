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
            categories: _categories.ToDomain(),
            couples: _couples.ToDomain()
        );
    }
}