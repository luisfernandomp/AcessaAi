using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using AcessaAi.Application.Usuarios.Interfaces;
using AcessaAi.Domain.GestaoUsuarios.Repositories;
using AcessaAi.Domain.Usuarios.Entities;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.Usuarios.Services
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioApplicationService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ErrorOr<UsuarioResponse>> CadastrarAsync(UsuariosCadastrarRequest request, CancellationToken cancellationToken)
        {
            var usuario = Usuario.CriarUsuario(request.Nome, request.Email, request.DataNascimento);

            if(usuario.IsError)
                return usuario.Errors;

            var usuarioResult = usuario.Value;

            usuarioResult.AdicionarEndereco(
                request.Endereco.Logradouro,
                request.Endereco.UF,
                request.Endereco.Cidade,
                request.Endereco.Numero,
                request.Endereco.CEP,
                request.Endereco.Bairro,
                request.Endereco.Complemento);

            var result = await _usuarioRepository.CriarAsync(usuarioResult, request.Senha, cancellationToken);

            if (result.IsError)
                return result.Errors;

            return result.Value.Adapt<UsuarioResponse>();
        }

        public async Task<ErrorOr<UsuarioResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id, cancellationToken);

            if (usuario is null)
                return Error.NotFound("Usuario.NaoEncontrado", "Usuário não encontrado.");

            return usuario.Adapt<UsuarioResponse>();
        }
    }
}
