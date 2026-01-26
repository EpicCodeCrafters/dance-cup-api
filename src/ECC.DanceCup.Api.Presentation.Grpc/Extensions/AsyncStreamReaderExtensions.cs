using ECC.DanceCup.Api.Presentation.Grpc.Validators;
using Grpc.Core;

namespace ECC.DanceCup.Api.Presentation.Grpc.Extensions;

public static class AsyncStreamReaderExtensions
{
    public static async Task<AddTournamentAttachmentRequest.Types.AttachmentInfo> ReadTournamentAttachmentInfoAsync(
        this IAsyncStreamReader<AddTournamentAttachmentRequest> stream)
    {
        if (!await stream.MoveNext())
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Не передана информация о файле"));
        }
        
        var current = stream.Current;
        if (current.AttachmentInfo is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Информация о файле должна быть передана первым чанком"));
        }
        
        var validationResult = await new AttachmentInfoValidator().ValidateAsync(current.AttachmentInfo);
        if (!validationResult.IsValid)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, validationResult.ToString()));
        }
        
        return current.AttachmentInfo;
    }

    public static async IAsyncEnumerable<byte[]> ReadTournamentAttachmentBytesAsync(
        this IAsyncStreamReader<AddTournamentAttachmentRequest> stream)
    {
        var chunksCount = 0;
        while (await stream.MoveNext())
        {
            var bytes = stream.Current.AttachmentBytes.ToByteArray();
            if (bytes is null or [])
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный пустой чанк"));
            }

            yield return bytes;
            
            ++chunksCount;
        }

        if (chunksCount == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Не передано ни одного чанка"));
        }
    }
}