using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.RecursosAcessibilidades.Dtos.Responses;
using AcessaAi.Application.Dtos;
using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using AcessaAi.Domain.Usuarios.Entities;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.RecursosAcessibilidades.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.Estabelecimentos.Consultas;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using Mapster;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;

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

            // Geocordenadas é um record com nomes iguais, mas precisa de config explícita
            TypeAdapterConfig<Geocordenadas, GeocordenadasResponse>.NewConfig();

            // QuantidadeEstrelas → Estrelas
            TypeAdapterConfig<Avaliacao, AvaliacaoResponse>
                .NewConfig()
                .Map(dest => dest.Estrelas, src => src.QuantidadeEstrelas);

            TypeAdapterConfig<EstabelecimentoFoto, EstabelecimentoFotoResponse>.NewConfig();

            // Geolocalizacao → Geocordenadas (nome diferente)
            TypeAdapterConfig<Estabelecimento, EstabelecimentoResponse>
                .NewConfig()
                .Map(dest => dest.Geocordenadas, src => src.Geolocalizacao)
                .Map(dest => dest.Fotos, src => src.Fotos == null
                    ? Enumerable.Empty<EstabelecimentoFotoResponse>()
                    : src.Fotos.Adapt<IEnumerable<EstabelecimentoFotoResponse>>());

            TypeAdapterConfig<RecursoAcessibilidade, RecursoAcessibilidadeResponse>.NewConfig();

            // Request → Consulta (para filtros)
            TypeAdapterConfig<EnderecoRequest, EnderecoConsulta>.NewConfig();
            TypeAdapterConfig<GeocordenadasRequest, GeocordenadasConsulta>.NewConfig();

            TypeAdapterConfig<EnderecoRequest, Endereco>
                .NewConfig()
                .ConstructUsing(src => new Endereco(src.Logradouro, src.UF, src.Cidade, src.Numero, src.CEP, src.Bairro, src.Complemento));

            TypeAdapterConfig<GeocordenadasRequest, Geocordenadas>
                .NewConfig()
                .ConstructUsing(src => new Geocordenadas(src.Latitude, src.Longitude));

            TypeAdapterConfig<EstabelecimentoFiltrarRequest, EstabelecimentoFiltrarConsulta>
                .NewConfig()
                .Map(dest => dest.EnderecoConsulta, src => src.EnderecoRequest)
                .Map(dest => dest.GeocordenadasConsulta, src => src.GeocordenadasRequest);

            TypeAdapterConfig.GlobalSettings.Compile();
        }
    }
}
