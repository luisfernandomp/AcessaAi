namespace AcessaAi.Application.Dtos.Requests
{
    public record class EnderecoRequest(string Logradouro, string UF, string Cidade, string Numero, string CEP, string Bairro, string Complemento);
}
