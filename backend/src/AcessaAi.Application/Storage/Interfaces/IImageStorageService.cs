namespace AcessaAi.Application.Storage.Interfaces;

public interface IImageStorageService
{
    Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken);

    Task DeleteAsync(string key, CancellationToken cancellationToken);

    string GetPresignedUrl(string key, int expirationMinutes = 60);
}
