using FluentResults;

namespace ECC.DanceCup.Api.Utils.Extensions;

public static class ResultExtensions
{
    public static string StringifyErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        return string.Join("; ", result.Errors.Select(error => error.Message));
    }
}