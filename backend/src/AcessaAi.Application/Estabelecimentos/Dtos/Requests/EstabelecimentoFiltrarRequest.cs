using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests;

public class EstabelecimentoFiltrarRequest
{
    public string? Nome { get; set; }
    public TipoEstabelecimento? Tipo { get; set; }
    public double? DistanciaMaxima { get; set; }
    public EnderecoRequest? EnderecoRequest { get; set; }
    public GeocordenadasRequest? GeocordenadasRequest { get; set; }
    public IEnumerable<int>? RecursosAcessabilidadeIds { get; set; }
}
