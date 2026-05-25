using AcessaAi.Application.Dtos;
using AcessaAi.Application.RecursosAcessibilidades.Dtos.Responses;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Responses
{
    public class EstabelecimentoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TipoEstabelecimento Tipo { get; set; }
        public GeocordenadasResponse Geocordenadas { get; set; }
        public IEnumerable<EstabelecimentoFotoResponse> Fotos { get; set; }
        public IEnumerable<RecursoAcessibilidadeResponse> RecursosAcessibilidade { get; set; }
        public EnderecoResponse Endereco { get; set; }
    }
}