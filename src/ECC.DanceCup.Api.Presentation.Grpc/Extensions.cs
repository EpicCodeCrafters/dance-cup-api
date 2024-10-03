using ECC.DanceCup.Api.Application.Errors;
using ECC.DanceCup.Api.Domain.Core;
using FluentResults;
using Grpc.Core;

namespace ECC.DanceCup.Api.Presentation.Grpc;

internal static class Extensions
{
    public static void HandleErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.HasError<NotFoundError>())
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.StringifyErrors()));
        }
        
        if (result.HasError<DomainError>())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.StringifyErrors()));
        }
        
        if (result.IsFailed)
        {
            throw new RpcException(new Status(StatusCode.Unknown, result.StringifyErrors()));
        }
    }

    public static string StringifyErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        return string.Join("; ", result.Errors.Select(error => error.Message));
    }
}