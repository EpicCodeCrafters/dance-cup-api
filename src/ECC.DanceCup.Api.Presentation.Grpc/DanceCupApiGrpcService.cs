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

    public override async Task<GetRefereesResponse> GetReferees(GetRefereesRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

        result.HandleErrors();

        return result.Value.ToGrpc();
    }

    public override async Task<CreateRefereeResponse> CreateReferee(CreateRefereeRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

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

    public override async Task<StartTournamentRegistrationResponse> StartTournamentRegistration(StartTournamentRegistrationRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

        result.HandleErrors();

        return new StartTournamentRegistrationResponse();
    }

    public override async Task<FinishTournamentRegistrationResponse> FinishTournamentRegistration(FinishTournamentRegistrationRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

        result.HandleErrors();

        return new FinishTournamentRegistrationResponse();
    }

    public override async Task<ReopenTournamentRegistrationResponse> ReopenTournamentRegistration(ReopenTournamentRegistrationRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

        result.HandleErrors();

        return new ReopenTournamentRegistrationResponse();
    }
}