using ErrorOr;

namespace AcessaAi.Application.Imagens.Interfaces;

public interface IImagemApplicationService
{
    Task<ErrorOr<string>> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);
}
