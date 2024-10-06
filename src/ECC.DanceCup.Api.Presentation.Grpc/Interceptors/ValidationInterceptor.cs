using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Presentation.Grpc.Interceptors;

public class ValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var requestValidator = _serviceProvider.GetService<IValidator<TRequest>>();
        await ValidateRequestAsync(request, requestValidator, context.CancellationToken);

        return await base.UnaryServerHandler(request, context, continuation);
    }

    private static async Task ValidateRequestAsync<TRequest>(TRequest request, IValidator<TRequest>? requestValidator, CancellationToken cancellationToken)
    {
        if (requestValidator is null)
        {
            return;
        }

        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, validationResult.ToString()));
        }
    }
}