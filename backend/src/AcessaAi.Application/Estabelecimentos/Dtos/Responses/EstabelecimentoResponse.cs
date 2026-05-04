
using AcessaAi.Application.Dtos;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Responses
{
    public class EstabelecimentoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public EnderecoResponse Endereco { get; set; }
    }
}