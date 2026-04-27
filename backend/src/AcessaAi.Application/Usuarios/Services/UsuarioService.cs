using AcessaAi.Application.Dtos;
using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using AcessaAi.Application.Usuarios.Interfaces;
using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Infrastructure.CrossCutting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcessaAi.Application.Usuarios.Services
{
    public class UsuarioService : IUsuarioService 
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioService(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<UsuarioResponse>> CadastrarAsync(UsuariosCadastrarRequest request, CancellationToken cancellationToken)
        {

            var usuario = Usuario.CriarUsuario(request.Nome, request.Email, request.DataNascimento);

            usuario.AdicionarEndereco(
                request.Endereco.Logradouro,
                request.Endereco.UF,
                request.Endereco.Cidade,
                request.Endereco.Numero,
                request.Endereco.CEP,
                request.Endereco.Bairro,
                request.Endereco.Complemento
            );

            var result = await _userManager.CreateAsync(usuario, request.Senha);

            var erros = result.Errors.Select(e => e.Description).ToArray();


            var usuarioResponse = result.Succeeded 
                ? TinyMapper.Map<UsuarioResponse>(usuario)
                : null;

            return new BaseResponse<UsuarioResponse>
            {
                Sucesso = result.Succeeded,
                Erros = erros,
                Resultado = usuarioResponse
            };
        }

        public async Task<BaseResponse<UsuarioResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return new BaseResponse<UsuarioResponse>
            {
                Sucesso = usuario.IsNotNull(),
                Erros = usuario.IsNull() ? new string[] { "Usuário não encontrado" } : [],
                Resultado = usuario.IsNull() ? null : TinyMapper.Map<UsuarioResponse>(usuario)
            };
        }
    }
}
