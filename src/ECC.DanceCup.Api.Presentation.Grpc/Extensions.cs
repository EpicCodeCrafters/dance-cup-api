using ECC.DanceCup.Api.Application.Errors;
using ECC.DanceCup.Api.Domain.Core;
using FluentResults;
using Grpc.Core;

namespace ECC.DanceCup.Api.Presentation.Grpc;

internal static class Extensions
{
    public static TResult HandleCommonErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.HasError<TournamentNotFoundError>())
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.StringifyErrors()));
        }
        
        if (result.HasError<DomainError>())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.StringifyErrors()));
        }
        
        return result;
    }
    
    public static TResult ThrowRpcUnknownIfError<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.IsFailed)
        {
            throw new RpcException(new Status(StatusCode.Unknown, result.StringifyErrors()));
        }

        return result;
    }

    public static string StringifyErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        return string.Join("; ", result.Errors.Select(error => error.Message));
    }
}