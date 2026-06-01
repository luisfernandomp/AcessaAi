using AcessaAi.Application.Imagens.Interfaces;
using AcessaAi.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagemController : ControllerBase
{
    private readonly IImagemApplicationService _imagemService;

    public ImagemController(IImagemApplicationService imagemService)
    {
        _imagemService = imagemService;
    }

    /// <summary>
    /// Envia uma imagem para o storage e retorna a chave gerada.
    /// Use a chave retornada nos campos <c>CapaChave</c> e <c>FotosChaves</c> ao criar um estabelecimento.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile imagem, CancellationToken cancellationToken)
    {
        using var stream = imagem.OpenReadStream();
        var result = await _imagemService.UploadAsync(stream, imagem.FileName, imagem.ContentType, cancellationToken);
        return result.ToActionResult(chave => Ok(new { chave }));
    }
}
