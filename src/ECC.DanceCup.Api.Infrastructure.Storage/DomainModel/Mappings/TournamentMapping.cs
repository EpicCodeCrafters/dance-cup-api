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
    public static Tournament ToDomain(this TournamentDbo dbo)
    {
        return new Tournament(
        UserId.From(dbo.UserId).AsRequired(),
        TournamentName.From(dbo.Name).AsRequired(),
        TournamentDescription.From(dbo.Description).AsRequired(),
        TournamentDate.From(dbo.Date).AsRequired(),
        Enum.Parse<TournamentState>(dbo.State),
        dbo.RegistrationStartedAt,
        dbo.RegistrationFinishedAt,
        dbo.StartedAt,
        dbo.FinishedAt,
        // _categories.AsRequired(),
        // _couples.AsRequired()
        );
    }
}