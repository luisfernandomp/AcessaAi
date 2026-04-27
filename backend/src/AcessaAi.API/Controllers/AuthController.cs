using AcessaAi.Application.Autenticacao.Dtos;
using AcessaAi.Application.Autenticacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AuthController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        /// <summary>
        /// Rota para realizar o login na aplicação. Recebe as credenciais do usuário e retorna um token de autenticação válido por um período determinado.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest dto, CancellationToken cancellationToken)
        {
            var response = await _autenticacaoService.LoginAsync(dto, cancellationToken);
            return Ok(response);
        }

    }
}
