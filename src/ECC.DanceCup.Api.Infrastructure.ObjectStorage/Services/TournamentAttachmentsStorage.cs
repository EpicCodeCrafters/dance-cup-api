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
        var key = string.Format(KeyTemplate, tournamentId.Value, attachmentNumber);
        var initiateMultipartUploadResponse = await s3Client.InitiateMultipartUploadAsync(BucketName, key, cancellationToken);

        try
        {
            var partNumber = 1;
            var partETags = new List<PartETag>();

            await foreach (var bytes in attachmentBytes.WithCancellation(cancellationToken))
            {
                using var memoryStream = new MemoryStream(bytes);
                var uploadPartResponse = await s3Client.UploadPartAsync(
                    new UploadPartRequest
                    {
                        BucketName = BucketName,
                        Key = key,
                        UploadId = initiateMultipartUploadResponse.UploadId,
                        PartNumber = partNumber,
                        InputStream = memoryStream
                    },
                    cancellationToken
                );

                partETags.Add(new PartETag(partNumber, uploadPartResponse.ETag));

                ++partNumber;
            }

            await s3Client.CompleteMultipartUploadAsync(new CompleteMultipartUploadRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    UploadId = initiateMultipartUploadResponse.UploadId,
                    PartETags = partETags
                },
                cancellationToken
            );
        }
        catch
        {
            await s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    UploadId = initiateMultipartUploadResponse.UploadId
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
}