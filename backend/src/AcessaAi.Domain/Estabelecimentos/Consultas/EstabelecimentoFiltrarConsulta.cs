using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Domain.Estabelecimentos.Consultas;

public class EstabelecimentoFiltrarConsulta
{
    public string? Nome { get; set; }
    public TipoEstabelecimento? Tipo { get; set; }
    public double? DistanciaMaxima { get; set; }
    public EnderecoConsulta? EnderecoConsulta { get; set; }
    public GeocordenadasConsulta? GeocordenadasConsulta { get; set; }
    public IEnumerable<int>? RecursosAcessabilidadeIds { get; set; }
}


