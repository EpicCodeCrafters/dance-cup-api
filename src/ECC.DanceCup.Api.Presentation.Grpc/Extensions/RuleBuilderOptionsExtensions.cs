using FluentValidation;

namespace ECC.DanceCup.Api.Presentation.Grpc.Extensions;

public static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<TProperty, T> UnlessNull<TProperty, T>(
        this IRuleBuilderOptions<TProperty, T> ruleBuilderOptions)
    {
        return ruleBuilderOptions.Unless(property => property is null);
    }
}