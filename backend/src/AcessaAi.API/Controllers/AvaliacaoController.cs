using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {


        public AvaliacaoController() { }

        [HttpGet]
        public async Task<IActionResult> RecuperarAvaliacaoAsync(CancellationToken cancellationToken)
        {
            return Ok("Avaliação realizada com sucesso!");
        }

        [HttpPost]
        public async Task<IActionResult> CriarAvaliacaoAsync(CancellationToken cancellationToken)
        {
            return Ok("Avaliação realizada com sucesso!");
        }

        [HttpPatch]
        public async Task<IActionResult> AtualizarAvaliacaoAsync(CancellationToken cancellationToken)
        {
            return Ok("Avaliação realizada com sucesso!");
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAvaliacaoAsync(CancellationToken cancellationToken)
        {
            return Ok("Avaliação realizada com sucesso!");
        }

    }
}
