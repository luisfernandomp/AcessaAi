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
        /// Retorna os dados de um usuário pelo ID. O campo UrlFotoPerfil retorna uma URL pré-assinada válida por 60 minutos.
        /// </summary>
        [Authorize]
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

        /// <summary>
        /// Atualiza os dados de um usuário (nome, data de nascimento, endereço).
        /// </summary>
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] UsuariosAtualizarRequest dto, CancellationToken cancellationToken)
        {
            var result = await _usuarioService.AtualizarAsync(id, dto, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Faz o upload da foto de perfil do usuário. Retorna a URL pré-assinada da imagem.
        /// </summary>
        [Authorize]
        [HttpPost("{id:int}/foto-perfil")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFotoPerfilAsync(int id, IFormFile foto, CancellationToken cancellationToken)
        {
            var result = await _usuarioService.UploadFotoPerfilAsync(
                id,
                foto.OpenReadStream(),
                foto.FileName,
                foto.ContentType,
                cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
