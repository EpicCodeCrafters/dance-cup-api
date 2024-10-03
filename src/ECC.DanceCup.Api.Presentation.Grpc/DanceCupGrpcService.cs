using Grpc.Core;
using MediatR;

namespace ECC.DanceCup.Api.Presentation.Grpc;

public class DanceCupGrpcService : DanceCupService.DanceCupServiceBase
{
    private readonly ISender _sender;

    public DanceCupGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetDancesResponse> GetDances(GetDancesRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

        result
            .HandleCommonErrors()
            .ThrowRpcUnknownIfError();

        return result.Value.ToGrpc();
    }
}