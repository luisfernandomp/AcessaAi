using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests
{
    public class EstabelecimentoCriarRequest
    {
        public string Nome { get; set; } = null!;
        public TipoEstabelecimento Tipo { get; set; }
        public GeocordenadasRequest Geocordenadas { get; set; } = null!;
        public EnderecoRequest Endereco { get; set; } = null!;
        public string? CapaChave { get; set; }
        public IEnumerable<string> FotosChaves { get; set; } = [];
        public IEnumerable<int> RecursosAcessibilidadesIds { get; set; } = [];
    }
}
