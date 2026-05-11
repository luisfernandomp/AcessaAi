using AcessaAi.Domain.Common;

namespace AcessaAi.Domain.Estabelecimentos.Consultas;

public class EstabelecimentoFiltrarConsulta
{
    public string? Nome { get; set; }
    public double? DistanciaMaxima { get; set; }
    public EnderecoConsulta? EnderecoConsulta { get; set; }
    public GeocordenadasConsulta? GeocordenadasConsulta { get; set; }
    public IEnumerable<int>? RecursosAcessabilidadeIds { get; set; }
}


