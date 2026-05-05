using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.Categorias.Dtos.Responses;
using AcessaAi.Application.Dtos;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using Mapster;

namespace AcessaAi.Application.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Usuario, UsuarioResponse>
                .NewConfig()
                .Map(dest => dest.Nome, src => src.NomeCompleto)
                .Map(dest => dest.DataNascimento, src => src.DataNascimento.DateTime);

            TypeAdapterConfig<Endereco, EnderecoResponse>.NewConfig();
            TypeAdapterConfig<Avaliacao, AvaliacaoResponse>.NewConfig();
            TypeAdapterConfig<Estabelecimento, EstabelecimentoResponse>.NewConfig();
            TypeAdapterConfig<Categoria, CategoriaResponse>.NewConfig();
        }
    }
}
