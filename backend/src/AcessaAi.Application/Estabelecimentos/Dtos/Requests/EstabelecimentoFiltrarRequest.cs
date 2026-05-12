using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests;

public class EstabelecimentoFiltrarRequest
{
    public string? Nome { get; set; }
    public double? DistanciaMaxima { get; set; }
    public EnderecoRequest? EnderecoRequest { get; set; }
    public GeocordenadasRequest? GeocordenadasRequest { get; set; }
    public IEnumerable<int>? RecursosAcessabilidadeIds { get; set; }
}
