using AcessaAi.Application.Autenticacao.Dtos;
using AcessaAi.Application.Autenticacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AuthController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest dto, CancellationToken cancellationToken)
        {
            var response = await _autenticacaoService.LoginAsync(dto, cancellationToken);
            return Ok(response);
        }

    }
}
