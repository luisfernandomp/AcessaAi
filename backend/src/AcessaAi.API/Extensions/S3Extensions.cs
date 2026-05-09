using System;
using AcessaAi.Application.Storage.Interfaces;
using AcessaAi.Infrastructure.Configurations;
using AcessaAi.Infrastructure.Storage;
using Amazon.Runtime;
using Amazon.S3;

namespace AcessaAi.API.Extensions;

public static class S3Extensions
{
    public static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IImageStorageService>(sp =>
        {
            var s3Settings = configuration.GetSection("S3Settings").Get<S3Settings>();

            var s3Config = new AmazonS3Config
            {
                ServiceURL = configuration["AWS:ServiceURL"],
                ForcePathStyle = true,
                AuthenticationRegion = configuration["AWS:Region"]
            };

            var credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"]
            );

            var client = new AmazonS3Client(credentials, s3Config);

            return new S3ImageStorageService(client, s3Settings);
        });

        return services;
    }
}
