using ECC.DanceCup.Api.Application.UseCases.AttachFileToTournament;
using ECC.DanceCup.Api.Application.UseCases.GetTournamentAttachment;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Presentation.Grpc.Extensions;
using ECC.DanceCup.Api.Utils.Extensions;
using Google.Protobuf;
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
    
    public override async Task<GetTournamentsResponse> GetTournaments(GetTournamentsRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

        result.HandleErrors();

        return result.Value.ToGrpc();
    }
    
    public override async Task<GetTournamentResponse> GetTournament(GetTournamentRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

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

    public override async Task<GetTournamentRegistrationResultResponse> GetTournamentRegistrationResult(GetTournamentRegistrationResultRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);
        
        result.HandleErrors();

        return result.Value.ToGrpc();
    }
    
    public override async Task<ReopenTournamentRegistrationResponse> ReopenTournamentRegistration(ReopenTournamentRegistrationRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);

        result.HandleErrors();

        return new ReopenTournamentRegistrationResponse();
    }

    public override async Task<RegisterCoupleForTournamentResponse> RegisterCoupleForTournament(RegisterCoupleForTournamentRequest request, ServerCallContext context)
    {
        var command = request.ToInternal();
        var result = await _sender.Send(command, context.CancellationToken);
        
        result.HandleErrors();

        return new RegisterCoupleForTournamentResponse();
    }

    public override async Task<AddTournamentAttachmentResponse> AddTournamentAttachment(IAsyncStreamReader<AddTournamentAttachmentRequest> requestStream, ServerCallContext context)
    {
        var attachmentInfo = await requestStream.ReadTournamentAttachmentInfoAsync();
        var command = new AttachFileToTournamentUseCase.Command(
            TournamentId.From(attachmentInfo.TournamentId).AsRequired(),
            attachmentInfo.Name,
            requestStream.ReadTournamentAttachmentBytesAsync()
        );
        
        var result = await _sender.Send(command, context.CancellationToken);
        
        result.HandleErrors();
        
        return new AddTournamentAttachmentResponse();
    }

    public override async Task<ListTournamentAttachmentsResponse> ListTournamentAttachments(ListTournamentAttachmentsRequest request, ServerCallContext context)
    {
        var query = request.ToInternal();
        var result = await _sender.Send(query, context.CancellationToken);

        result.HandleErrors();

        return result.Value.ToGrpc();
    }

    public override async Task GetTournamentAttachment(
        GetTournamentAttachmentRequest request, 
        IServerStreamWriter<GetTournamentAttachmentResponse> responseStream,
        ServerCallContext context)
    {
        var query = new GetTournamentAttachmentUseCase.Query(
            TournamentId.From(request.TournamentId).AsRequired(),
            request.AttachmentNumber,
            request.MaxBytesCount
        );
        
        var result = await _sender.Send(query, context.CancellationToken);
        
        result.HandleErrors();

        await responseStream.WriteAsync(new GetTournamentAttachmentResponse
        {
            AttachmentInfo = new GetTournamentAttachmentResponse.Types.AttachmentInfo
            {
                Name = result.Value.AttachmentName,
                TotalBytesCount = result.Value.TotalAttachmentBytesCount
            }
        }, context.CancellationToken);

        await foreach (var bytes in result.Value.AttachmentBytes.WithCancellation(context.CancellationToken))
        {
            await responseStream.WriteAsync(new GetTournamentAttachmentResponse
            {
                AttachmentBytes = ByteString.CopyFrom(bytes)
            }, context.CancellationToken);
        }
    }
}