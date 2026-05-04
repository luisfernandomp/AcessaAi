using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Responses
{
    public class EstabelecimentoCriarRequest
    {
        public string Nome { get; set; }    
        public GeocordenadasRequest Geocordenadas { get; set; }
    }
}