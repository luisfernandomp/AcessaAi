using System.Globalization;
using AcessaAi.API.Extensions;
using AcessaAi.API.Requests;
using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcessaAi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabelecimentoController : ControllerBase
    {
        private readonly IEstabelecimentoApplicationService _estabelecimentoService;

        public EstabelecimentoController(IEstabelecimentoApplicationService estabelecimentoService)
        {
            _estabelecimentoService = estabelecimentoService;
        }

        /// <summary>
        /// Cria um novo estabelecimento com fotos (multipart/form-data).
        /// Envie <c>Capa</c> para a foto de capa e zero ou mais arquivos <c>Fotos</c> para o carrossel.
        /// </summary>
        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CriarAsync([FromForm] EstabelecimentoCriarFormRequest form, CancellationToken cancellationToken)
        {
            var request = new EstabelecimentoCriarRequest
            {
                Nome = form.Nome,
                Tipo = form.Tipo,
                Geocordenadas = new GeocordenadasRequest {
                    Latitude = double.Parse(form.Latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(form.Longitude, CultureInfo.InvariantCulture)
                    },
                Endereco = new EnderecoRequest(form.Logradouro, form.UF, form.Cidade, form.Numero, form.CEP, form.Bairro, form.Complemento),
                Capa = form.Capa?.ToEstabelecimentoImagemRequest(isCapa: true),
                Fotos = form.Fotos?.Select(f => f.ToEstabelecimentoImagemRequest()) ?? [],
                RecursosAcessibilidadesIds = form.RecursosAcessibilidadesIds 
            };

            var result = await _estabelecimentoService.CriarAsync(request, cancellationToken);
            return result.ToActionResult(estabelecimento =>
                CreatedAtAction("ObterPorId", new { id = estabelecimento.Id }, estabelecimento));
        }


        /// <summary>
        /// Subir uma imagem para um estabelecimento existente. A imagem é enviada como um arquivo multipart/form-data e associada ao estabelecimento pelo ID.
        /// Use <paramref name="isCapa"/> para indicar se a imagem é a capa do estabelecimento; caso contrário, será adicionada ao carrossel.
        /// </summary>
        [Authorize]
        [HttpPost("{id:int}/imagem")]
        public async Task<IActionResult> SubirImagemAsync(int id, IFormFile imagem, [FromQuery] bool isCapa = false, CancellationToken cancellationToken = default)
        {
            var request = imagem.ToEstabelecimentoImagemRequest(isCapa);
            var result = await _estabelecimentoService.SubirImagemAsync(id, request, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Atualiza um estabelecimento existente.
        /// </summary>
        [Authorize]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> AtualizarAsync(
            int id,
            [FromBody] EstabelecimentoAtualizarRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.AtualizarAsync(id, request, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Remove um estabelecimento pelo ID.
        /// </summary>
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ExcluirAsync(id, cancellationToken);
            return result.ToActionResult(_ => NoContent());
        }

        /// <summary>
        /// Retorna um estabelecimento pelo ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.ObterPorIdAsync(id, cancellationToken);
            return result.ToActionResult(Ok);
        }

        /// <summary>
        /// Filtra estabelecimentos por nome, recursos de acessibilidade e/ou distância máxima a partir de coordenadas geográficas.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FiltrarAsync([FromQuery] EstabelecimentoFiltrarRequest request, CancellationToken cancellationToken)
        {
            var result = await _estabelecimentoService.FiltrarAsync(request, cancellationToken);
            return result.ToActionResult(Ok);
        }
    }
}
