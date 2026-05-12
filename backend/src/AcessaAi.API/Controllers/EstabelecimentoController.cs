using AcessaAi.API.Extensions;
using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabelecimentoController : ControllerBase
    {
        private readonly IEstabelecimentoApplicationService _estabelecimentoService;

        public EstabelecimentoController(IEstabelecimentoApplicationService estabelecimentoService)
        {
            _estabelecimentoService = estabelecimentoService;
        }

        /// <summary>
        /// Cria um novo estabelecimento.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarAsync([FromBody] EstabelecimentoCriarRequest request, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.CriarAsync(request, cancellationToken);
            return result.ToActionResult(estabelecimento =>
                CreatedAtAction("ObterPorId", new { id = estabelecimento.Id }, estabelecimento));
        }


        /// <summary>
        /// Subir uma imagem para um estabelecimento existente. A imagem é enviada como um arquivo multipart/form-data e associada ao estabelecimento pelo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imagem"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{id:int}/imagem")]
        public async Task<IActionResult> SubirImagemAsync(int id, IFormFile imagem, CancellationToken cancellationToken)
        {
            var request = imagem.ToEstabelecimentoImagemRequest();   
            var result = await _estabelecimentoService.SubirImagemAsync(id, request, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Atualiza um estabelecimento existente.
        /// </summary>
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> AtualizarAsync(
            int id,
            [FromBody] EstabelecimentoAtualizarRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.AtualizarAsync(id, request, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Remove um estabelecimento pelo ID.
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ExcluirAsync(id, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Retorna um estabelecimento pelo ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Filtra estabelecimentos por nome, recursos de acessibilidade e/ou distância máxima a partir de coordenadas geográficas.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FiltrarAsync([FromQuery] EstabelecimentoFiltrarRequest request, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.FiltrarAsync(request, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
