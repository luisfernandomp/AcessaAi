using AcessaAi.Application.Imagens.Interfaces;
using AcessaAi.Application.Storage.Interfaces;
using ErrorOr;

namespace AcessaAi.Application.Imagens.Services;

public class ImagemApplicationService : IImagemApplicationService
{
    private readonly IImageStorageService _imageStorageService;

    public ImagemApplicationService(IImageStorageService imageStorageService)
    {
        _imageStorageService = imageStorageService;
    }

    public async Task<ErrorOr<string>> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var chave = await _imageStorageService.UploadAsync(content, fileName, contentType, cancellationToken);
        return chave;
    }
}
