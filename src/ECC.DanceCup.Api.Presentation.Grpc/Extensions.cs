using ECC.DanceCup.Api.Domain.Core;
using FluentResults;
using Grpc.Core;

namespace ECC.DanceCup.Api.Presentation.Grpc;

internal static class Extensions
{
    public static TResult HandleCommonErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.HasError<DomainError>())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.ToString() ?? string.Empty));
        }
        
        return result;
    }
    
    public static TResult ThrowRpcUnknownIfError<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.IsFailed)
        {
            throw new RpcException(new Status(StatusCode.Unknown, result.ToString() ?? string.Empty));
        }

        return result;
    }
}