using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests
{
    public class EstabelecimentoAtualizarRequest
    {
        public string Nome { get; set; } = null!;
        public TipoEstabelecimento? Tipo { get; set; }
        public GeocordenadasRequest Geocordenadas { get; set; } = null!;
    }
}
