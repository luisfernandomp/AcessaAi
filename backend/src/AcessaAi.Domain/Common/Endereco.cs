namespace AcessaAi.Domain.Common
{
    public class Endereco
    {
        public string Logradouro { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;  
        public string? Complemento { get; set; }

        private Endereco() { }

        public Endereco(string logradouro, string uf, string cidade, string numero, string cep, string bairro, string? complemento = null)
        {
            Logradouro = logradouro;
            UF = uf;
            Cidade = cidade;
            Numero = numero;
            CEP = cep;
            Bairro = bairro;
            Complemento = complemento;
        }
    }
}
