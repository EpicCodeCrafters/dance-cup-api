using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;

namespace ECC.DanceCup.Api.Presentation.Grpc.Extensions;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilder<TProperty, long> IsValidUserId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .Must(value => UserId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор пользователя");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, long> IsValidDanceId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .Must(value => DanceId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор танца");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, long> IsValidRefereeId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .Must(value => RefereeId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор судьи");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, string> IsValidRefereeFullName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .Must(value => RefereeFullName.From(value) is not null)
            .WithMessage("Необходимо передать корректое полное имя судьи");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, long> IsValidTournamentId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .Must(value => TournamentId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор турнира");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, string> IsValidTournamentName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .Must(value => TournamentName.From(value) is not null)
            .WithMessage("Необходимо передать корректное название турнира");

        return ruleBuilder;
    }

    public static IRuleBuilder<TProperty, string> IsValidTournamentDescription<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .Must(value => TournamentDescription.From(value) is not null)
            .WithMessage("Необходимо передать корректное описание турнира");

        return ruleBuilder;
    }

    public static IRuleBuilder<TProperty, Timestamp> IsValidTournamentDate<TProperty>(this IRuleBuilder<TProperty, Timestamp> ruleBuilder)
    {
        ruleBuilder
            .Must(value => TournamentDate.From(value.ToDateTime()) is not null)
            .WithMessage("Необходимо передать корректную дату турнира");

        return ruleBuilder;
    }
    
    
    public static IRuleBuilder<TProperty, long> IsValidCategoryId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .Must(value => DanceId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор категории");

        return ruleBuilder;
    }
    
    public static IRuleBuilder<TProperty, string> IsValidCategoryName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .Must(value => CategoryName.From(value) is not null)
            .WithMessage("Необходимо передать корректное название категории");

        return ruleBuilder;
    }
    
    public static IRuleBuilderOptions<TProperty, string> IsValidCoupleParticipantFullName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => CoupleParticipantFullName.From(value) is not null)
            .WithMessage("Необходимо передать корректное полное имя участника пары");
    }
    
    public static IRuleBuilderOptions<TProperty, string> IsValidCoupleDanceOrganizationName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => CoupleDanceOrganizationName.From(value) is not null)
            .WithMessage("Необходимо передать корректное название организации, в которой пара занимается танцами");
    }
    
    public static IRuleBuilderOptions<TProperty, string> IsValidCoupleTrainerFullName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => CoupleTrainerFullName.From(value) is not null)
            .WithMessage("Необходимо передать корректное полное имя тренера пары");
    }
}