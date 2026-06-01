using AcessaAi.Application.Dtos.Requests;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests;

// Suporta coordenadas tanto planas (?latitude=x&longitude=y)
// quanto aninhadas (?geocordenadasRequest.latitude=x&geocordenadasRequest.longitude=y)
public class GeoFiltroRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class EstabelecimentoFiltrarRequest
{
    public string? Nome { get; set; }
    public TipoEstabelecimento? Tipo { get; set; }
    public double? DistanciaMaxima { get; set; }
    public EnderecoRequest? EnderecoRequest { get; set; }

    // formato novo: ?latitude=x&longitude=y
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // formato legado: ?geocordenadasRequest.latitude=x&geocordenadasRequest.longitude=y
    public GeoFiltroRequest? GeocordenadasRequest { get; set; }

    public IEnumerable<int>? RecursosAcessabilidadeIds { get; set; }

    public double? LatitudeResolvida => Latitude ?? GeocordenadasRequest?.Latitude;
    public double? LongitudeResolvida => Longitude ?? GeocordenadasRequest?.Longitude;
}
