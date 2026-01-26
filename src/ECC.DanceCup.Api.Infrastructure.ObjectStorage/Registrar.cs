using Amazon.Runtime;
using Amazon.S3;
using ECC.DanceCup.Api.Application.Abstractions.ObjectStorage;
using ECC.DanceCup.Api.Infrastructure.ObjectStorage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Api.Infrastructure.ObjectStorage;

public static class Registrar
{
    public static IServiceCollection AddObjectStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITournamentAttachmentsStorage, TournamentAttachmentsStorage>();
        
        var accessKey = configuration["ObjectStorage:AccessKey"];
        var secretKey = configuration["ObjectStorage:SecretKey"];
        var serviceUrl = configuration["ObjectStorage:ServiceURL"];

        var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
        var s3Config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true
        };

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(awsCredentials, s3Config));
        
        return services;
    }
}