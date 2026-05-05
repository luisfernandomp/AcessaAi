using AcessaAi.API.Extensions;
using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplicationService _usuarioService;

        public UsuarioController(IUsuarioApplicationService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Retorna os dados de um usuário pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _usuarioService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarAsync([FromBody] UsuariosCadastrarRequest dto, CancellationToken cancellationToken)
        {
            var result = await _usuarioService.CadastrarAsync(dto, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
