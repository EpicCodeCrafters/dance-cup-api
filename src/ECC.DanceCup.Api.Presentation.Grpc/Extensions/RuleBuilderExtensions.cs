using ECC.DanceCup.Api.Domain.Model;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;

namespace ECC.DanceCup.Api.Presentation.Grpc.Extensions;

internal static class RuleBuilderExtensions
{
    public static void IsValidUserId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать идентификатор пользователя");

        ruleBuilder
            .Must(value => UserId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор пользователя");
    }
    
    public static void IsValidDanceId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать идентификатор танца");

        ruleBuilder
            .Must(value => DanceId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор танца");
    }
    
    public static void IsValidRefereeId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать идентификатор судьи");

        ruleBuilder
            .Must(value => RefereeId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор судьи");
    }
    
    public static void IsValidTournamentId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать идентификатор турнира");

        ruleBuilder
            .Must(value => TournamentId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор турнира");
    }
    
    public static void IsValidTournamentName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать название турнира");

        ruleBuilder
            .Must(value => TournamentName.From(value) is not null)
            .WithMessage("Необходимо передать корректное название турнира");
    }
    
    public static void IsValidTournamentDate<TProperty>(this IRuleBuilder<TProperty, Timestamp> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать дату турнира");
 
        ruleBuilder
            .Must(value => TournamentDate.From(value.ToDateTime()) is not null)
            .WithMessage("Необходимо передать корректную дату турнира");
    }
    
    
    public static void IsValidCategoryId<TProperty>(this IRuleBuilder<TProperty, long> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать идентификатор категории");

        ruleBuilder
            .Must(value => DanceId.From(value) is not null)
            .WithMessage("Необходимо передать корректный идентификатор категории");
    }
    
    public static void IsValidCategoryName<TProperty>(this IRuleBuilder<TProperty, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать название категории");

        ruleBuilder
            .Must(value => CategoryName.From(value) is not null)
            .WithMessage("Необходимо передать корректное название категории");
    }
}