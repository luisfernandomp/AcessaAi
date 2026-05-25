using AcessaAi.Application.Storage.Interfaces;
using AcessaAi.Infrastructure.Configurations;
using Amazon.S3;
using Amazon.S3.Model;

namespace AcessaAi.Infrastructure.Storage;

public class S3ImageStorageService : IImageStorageService
{
    private readonly S3Settings _s3Settings;
    private readonly AmazonS3Client _s3Client;
    public S3ImageStorageService(
        AmazonS3Client client,
        S3Settings s3Settings)
    {
        _s3Client = client;
        _s3Settings = s3Settings;
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var bucket = _s3Settings.BucketName;
        var key = $"uploads/{Guid.NewGuid()}-{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType,
            UseChunkEncoding = false,
        };

        var result = await _s3Client.PutObjectAsync(request);

        if(result.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Erro ao fazer upload da imagem para o S3.");
        }

        return key;
    }

    public Task DeleteAsync(string key, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public string GetPresignedUrl(string key, int expirationMinutes = 60)
    {
        if (string.IsNullOrEmpty(key))
            return key;

        if (key.StartsWith("http://") || key.StartsWith("https://"))
            return key;

        if (!string.IsNullOrEmpty(_s3Settings.PublicBaseUrl))
            return $"{_s3Settings.PublicBaseUrl.TrimEnd('/')}/{key}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Verb = HttpVerb.GET,
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
