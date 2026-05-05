using AcessaAi.Application.Storage.Interfaces;

namespace AcessaAi.Infrastructure.Storage;

public class S3ImageStorageService : IImageStorageService
{
    public Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task DeleteAsync(string key, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
