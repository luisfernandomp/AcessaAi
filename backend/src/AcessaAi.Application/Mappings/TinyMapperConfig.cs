
using AcessaAi.Application.Dtos;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Common;
using Nelibur.ObjectMapper;

namespace AcessaAi.Application.Mappings
{
    public static class TinyMapperConfig
    {
        public static void RegisterMappings()
        {
            // Mapeamento de Endereco para EnderecoResponse
            Nelibur.ObjectMapper.TinyMapper.Bind<Endereco, EnderecoResponse>();

            // Mapeamento de Usuario para UsuarioResponse
            Nelibur.ObjectMapper.TinyMapper.Bind<Usuario, UsuarioResponse>();
        }
    }
}
