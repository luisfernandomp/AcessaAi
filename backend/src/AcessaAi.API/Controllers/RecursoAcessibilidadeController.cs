using AcessaAi.API.Extensions;
using AcessaAi.Application.RecursosAcessibilidades.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoAcessibilidadeController : ControllerBase
    {
        private readonly IRecursoAcessibilidadeApplicationService _recursoAcessibilidadeApplicationService;
        public RecursoAcessibilidadeController(IRecursoAcessibilidadeApplicationService recursoAcessibilidadeApplicationService)
        {
            _recursoAcessibilidadeApplicationService = recursoAcessibilidadeApplicationService;
        }

        [HttpGet("listar-ativas")]
        public async Task<IActionResult> ListarRecursosAcessibilidadesAtivasAsync(CancellationToken cancellationToken)
        {
            var recursosAcessibilidades = await _recursoAcessibilidadeApplicationService.ListarRecursosAcessibilidadesAtivasAsync(cancellationToken);
            return  recursosAcessibilidades.ToActionResult(Ok);
        }
    }
}
