using AcessaAi.API.Extensions;
using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoApplicationService _avaliacaoService;

        public AvaliacaoController(IAvaliacaoApplicationService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        /// <summary>
        /// Cria uma nova avaliação. O endpoint espera um objeto do tipo AvaliacaoCreateRequest no corpo da requisição, contendo os detalhes da avaliação a ser criada. Se a criação for bem-sucedida, retorna um status 201 Created com os detalhes da avaliação criada. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CriarAsync([FromBody] AvaliacaoCreateRequest request, CancellationToken cancellationToken)
        {
            var result = await _avaliacaoService.CriarAsync(request, cancellationToken);
            
            return result.ToActionResult(avaliacao => 
                CreatedAtAction(nameof(ObterPorIdAsync).Replace("Async", ""), new { id = avaliacao.Id }, avaliacao)
            );
        }

        /// <summary>
        /// Altera uma avaliação existente. O endpoint espera um ID de avaliação como parâmetro de rota e um objeto do tipo AvaliacaoUpdateRequest no corpo da requisição, contendo os detalhes atualizados da avaliação. Se a atualização for bem-sucedida, retorna um status 200 OK com os detalhes da avaliação atualizada. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Atualizar(
            int id, 
            [FromBody] AvaliacaoUpdateRequest request, 
            CancellationToken cancellationToken)
        {
            request.Id = id;
            var result = await _avaliacaoService.AtualizarAsync(request, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Exclui uma avaliação existente. O endpoint espera um ID de avaliação como parâmetro de rota. Se a exclusão for bem-sucedida, retorna um status 204 No Content. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _avaliacaoService.ExcluirAsync(id, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Obtém uma avaliação por ID. O endpoint espera um ID de avaliação como parâmetro de rota. Se a avaliação for encontrada, retorna um status 200 OK com os detalhes da avaliação. Caso contrário, retorna um status de erro apropriado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _avaliacaoService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
