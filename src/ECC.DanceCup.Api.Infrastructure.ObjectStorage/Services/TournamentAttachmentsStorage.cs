using System.Runtime.CompilerServices;
using Amazon.S3;
using Amazon.S3.Model;
using ECC.DanceCup.Api.Application.Abstractions.ObjectStorage;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

namespace ECC.DanceCup.Api.Infrastructure.ObjectStorage.Services;

public class TournamentAttachmentsStorage(
    IAmazonS3 s3Client
) : ITournamentAttachmentsStorage
{
    private const string BucketName = "tournament-attachments";
    private const string KeyTemplate = "{0}/{1}";

public async Task PutAttachmentAsync(
    TournamentId tournamentId,
    int attachmentNumber,
    IAsyncEnumerable<byte[]> attachmentBytes,
    CancellationToken cancellationToken)
{
    const int minPartSize = 5 * 1024 * 1024;

    var key = string.Format(KeyTemplate, tournamentId.Value, attachmentNumber);

    var initiateResponse = await s3Client.InitiateMultipartUploadAsync(BucketName, key, cancellationToken);

    var uploadId = initiateResponse.UploadId;
    var partNumber = 1;
    var partETags = new List<PartETag>();

    using var bufferStream = new MemoryStream(capacity: minPartSize);

    try
    {
        await foreach (var chunk in attachmentBytes.WithCancellation(cancellationToken))
        {
            await bufferStream.WriteAsync(chunk, cancellationToken);

            if (bufferStream.Length < minPartSize)
            {
                continue;
            }

            bufferStream.Position = 0;

            var uploadPartResponse = await s3Client.UploadPartAsync(
                new UploadPartRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    UploadId = uploadId,
                    PartNumber = partNumber,
                    InputStream = bufferStream,
                    PartSize = bufferStream.Length
                },
                cancellationToken
            );

            partETags.Add(new PartETag(partNumber, uploadPartResponse.ETag));
            ++partNumber;

            bufferStream.SetLength(0);
            bufferStream.Position = 0;
        }

        if (bufferStream.Length > 0)
        {
            bufferStream.Position = 0;

            var uploadPartResponse = await s3Client.UploadPartAsync(
                new UploadPartRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    UploadId = uploadId,
                    PartNumber = partNumber,
                    InputStream = bufferStream,
                    PartSize = bufferStream.Length
                },
                cancellationToken
            );

            partETags.Add(new PartETag(partNumber, uploadPartResponse.ETag));
        }

        await s3Client.CompleteMultipartUploadAsync(
            new CompleteMultipartUploadRequest
            {
                BucketName = BucketName,
                Key = key,
                UploadId = uploadId,
                PartETags = partETags
            },
            cancellationToken
        );
    }
    catch
    {
        await s3Client.AbortMultipartUploadAsync(
            new AbortMultipartUploadRequest
            {
                BucketName = BucketName,
                Key = key,
                UploadId = uploadId
            },
            cancellationToken
        );

        throw;
    }
}

    public async Task DeleteAttachment(
        TournamentId tournamentId,
        int attachmentNumber,
        CancellationToken cancellationToken)
    {
        var key = string.Format(KeyTemplate, tournamentId, attachmentNumber);
        await s3Client.DeleteObjectAsync(BucketName, key, cancellationToken);
    }

    public async Task<long> GetTotalAttachmentBytesCount(TournamentId tournamentId, int attachmentNumber, CancellationToken cancellationToken)
    {
        var key = string.Format(KeyTemplate, tournamentId.Value, attachmentNumber);
        var getObjectMetadataResponse = await s3Client.GetObjectMetadataAsync(BucketName, key, cancellationToken);
        
        return getObjectMetadataResponse.ContentLength;
    }

    public async IAsyncEnumerable<byte[]> GetAttachmentAsync(
        TournamentId tournamentId,
        int attachmentNumber,
        int maxBytesCount,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var key = string.Format(KeyTemplate, tournamentId.Value, attachmentNumber);

        var response = await s3Client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = BucketName,
            Key = key
        }, cancellationToken);

        await using var stream = response.ResponseStream;

        maxBytesCount = (int)Math.Min(maxBytesCount, response.ContentLength);
        var buffer = new byte[maxBytesCount];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            var chunk = new byte[bytesRead];
            Buffer.BlockCopy(buffer, 0, chunk, 0, bytesRead);

            yield return chunk;
        }
    }}