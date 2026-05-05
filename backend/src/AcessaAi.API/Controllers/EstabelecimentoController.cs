using AcessaAi.API.Extensions;
using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Interfaces;
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
        [HttpPost]
        public async Task<IActionResult> CriarAsync([FromBody] EstabelecimentoCriarRequest request, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.CriarAsync(request, cancellationToken);
            return result.ToActionResult(estabelecimento =>
                CreatedAtAction(nameof(ObterPorIdAsync), new { id = estabelecimento.Id }, estabelecimento));
        }

        /// <summary>
        /// Atualiza um estabelecimento existente.
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> AtualizarAsync(
            int id,
            [FromBody] EstabelecimentoAtualizarRequest request,
            CancellationToken cancellationToken)
        {
            request.Id = id;
            var result = await _estabelecimentoService.AtualizarAsync(request, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Remove um estabelecimento pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ExcluirAsync(id, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Retorna um estabelecimento pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
