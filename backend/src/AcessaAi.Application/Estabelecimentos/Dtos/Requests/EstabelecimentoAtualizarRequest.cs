using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Responses
{
    public class EstabelecimentoAtualizarRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }    
        public GeocordenadasRequest Geocordenadas { get; set; }

    }
}