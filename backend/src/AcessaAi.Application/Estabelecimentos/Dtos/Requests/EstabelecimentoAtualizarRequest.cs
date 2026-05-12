using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests
{
    public class EstabelecimentoAtualizarRequest
    {
        public string Nome { get; set; } = null!;
        public GeocordenadasRequest Geocordenadas { get; set; } = null!;
    }
}
