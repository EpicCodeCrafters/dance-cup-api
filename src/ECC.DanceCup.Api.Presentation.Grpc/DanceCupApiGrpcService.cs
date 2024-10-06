using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using Grpc.Core;
using MediatR;

namespace ECC.DanceCup.Api.Presentation.Grpc;

public class DanceCupApiGrpcService : DanceCupApi.DanceCupApiBase
{
    private readonly ISender _sender;

    public DanceCupApiGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetDancesResponse> GetDances(GetDancesRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

        result.HandleErrors();

        return result.Value.ToGrpc();
    }

    public override async Task<CreateTournamentResponse> CreateTournament(CreateTournamentRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

        result.HandleErrors();

        return result.Value.ToGrpc();
    }
}