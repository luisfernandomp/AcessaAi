using AcessaAi.API.Extensions;
using AcessaAi.Application.Categorias.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaApplicationService _categoriaApplicationService;
        public CategoriaController(ICategoriaApplicationService categoriaApplicationService)
        {
            _categoriaApplicationService = categoriaApplicationService;
        }

        [Authorize]
        [HttpGet("listar-ativas")]
        public async Task<IActionResult> ListarCategoriasAtivasAsync(CancellationToken cancellationToken)
        {
            var categorias = await _categoriaApplicationService.ListarCategoriasAtivasAsync(cancellationToken);
            return  categorias.ToActionResult(Ok);
        }
    }
}
