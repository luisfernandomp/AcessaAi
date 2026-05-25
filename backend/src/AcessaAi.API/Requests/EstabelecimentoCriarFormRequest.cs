using AcessaAi.Domain.GestaoEstabelecimentos.Enums;

namespace AcessaAi.API.Requests;

public class EstabelecimentoCriarFormRequest
{
    public string Nome { get; set; } = null!;
    public TipoEstabelecimento Tipo { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string Logradouro { get; set; } = null!;
    public string UF { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string CEP { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string? Complemento { get; set; }

    public IFormFile? Capa { get; set; }
    public IFormFileCollection? Fotos { get; set; }
}
