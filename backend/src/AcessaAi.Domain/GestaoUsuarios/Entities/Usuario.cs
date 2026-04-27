using AcessaAi.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AcessaAi.Domain.Autenticacao.Entities
{
    public class Usuario : IdentityUser<int>
    {
        public string NomeCompleto { get; set; } = null!;
        public Endereco? Endereco { get; set; }
        public DateTimeOffset DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UltimoLogin { get; set; }

        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

        private Usuario() { }

        public static Usuario CriarUsuario(string nomeCompleto, string email, DateTimeOffset dataNascimento)
        {
            return new Usuario
            {
                UserName = email,
                Email = email,
                NomeCompleto = nomeCompleto,
                DataNascimento = dataNascimento
            };
        }

        public void AdicionarEndereco(string logradouro, string uf, string cidade, string numero, string cep, string bairro, string complemento)
        {
            Endereco = new Endereco(logradouro, uf, cidade, numero, cep, bairro, complemento);
        }

    }
}
