namespace AcessaAi.Application.Dtos
{
    public record class EnderecoResponse
    {
        public string Logradouro { get; init; } = string.Empty;
        public string UF { get; init; } = string.Empty;
        public string Cidade { get; init; } = string.Empty;
        public string Numero { get; init; } = string.Empty;
        public string CEP { get; init; } = string.Empty;
        public string Bairro { get; init; } = string.Empty;
        public string Complemento { get; init; } = string.Empty;
    }
}
