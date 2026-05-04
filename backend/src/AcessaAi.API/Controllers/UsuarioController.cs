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
        /// Rota para recuperar um usuário pelo seu ID. Retorna os detalhes do usuário correspondente ao ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _usuarioService.ObterPorIdAsync(id, cancellationToken);

            if (!response.Sucesso)
                return NotFound(response);

            return Ok(response);
        }

        /// <summary>
        /// Rota para cadastrar um novo usuário. Recebe os dados do usuário no corpo da requisição e cria um novo registro de usuário no sistema.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarAsync([FromBody] UsuariosCadastrarRequest dto, CancellationToken cancellationToken)
        {
            var usuarioResponse = await _usuarioService.CadastrarAsync(dto, cancellationToken);
            return Ok(usuarioResponse);
        }
    }
}
