using AcessaAi.API.Extensions;
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
        private readonly IAutenticacaoApplicationService _autenticacaoService;

        public AuthController(IAutenticacaoApplicationService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        /// <summary>
        /// Realiza o login e retorna um token JWT válido.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest dto, CancellationToken cancellationToken)
        {
            var result = await _autenticacaoService.LoginAsync(dto, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
