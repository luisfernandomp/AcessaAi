using AcessaAi.API.Extensions;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Application.Avaliacoes.Services;
using Microsoft.AspNetCore.Mvc;
using AcessaAi.Application.Estabelecimentos.Services;

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
        /// Cria um novo estabelecimento. O endpoint espera um objeto do tipo EstabelecimentoCriarRequest no corpo da requisição, contendo os detalhes do estabelecimento a ser criado. Se a criação for bem-sucedida, retorna um status 201 Created com os detalhes do estabelecimento criado. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CriarAsync([FromBody] EstabelecimentoCriarRequest request, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.CriarAsync(request, cancellationToken);

            return result.ToActionResult(estabelecimento =>
                CreatedAtAction(nameof(ObterPorIdAsync), new { id = estabelecimento.Id }, estabelecimento)
            );
        }

        /// <summary>
        /// Altera um estabelecimento existente. O endpoint espera um ID de estabelecimento como parâmetro de rota e um objeto do tipo EstabelecimentoAtualizarRequest no corpo da requisição, contendo os detalhes atualizados do estabelecimento. Se a atualização for bem-sucedida, retorna um status 200 OK com os detalhes do estabelecimento atualizado. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// Exclui um estabelecimento existente. O endpoint espera um ID de estabelecimento como parâmetro de rota. Se a exclusão for bem-sucedida, retorna um status 204 No Content. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ExcluirAsync(id, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Obtém um estabelecimento por ID. O endpoint espera um ID de estabelecimento como parâmetro de rota. Se o estabelecimento for encontrado, retorna um status 200 OK com os detalhes do estabelecimento. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
