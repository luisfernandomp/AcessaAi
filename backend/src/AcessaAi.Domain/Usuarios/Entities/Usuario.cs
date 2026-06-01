using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace AcessaAi.Domain.Usuarios.Entities
{
    public class Usuario : IdentityUser<int>
    {
        public string NomeCompleto { get; private set; } = null!;
        public Endereco? Endereco { get; private set; }
        public DateTimeOffset DataNascimento { get; private set; }
        public bool Ativo { get; private set; } = true;
        public DateTimeOffset DataCadastro { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UltimoLogin { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; private set; }
        public string? UrlFotoPerfil { get; private set; }
        public ICollection<Avaliacao> Avaliacoes { get; private set; }
        
        private Usuario() { }

        public Usuario(string nomeCompleto, string email, DateTimeOffset dataNascimento)
        {
            UserName = email;
            Email = email;
            NomeCompleto = nomeCompleto;
            DataNascimento = dataNascimento;
        }

        public static ErrorOr<Usuario> CriarUsuario(string nomeCompleto, string email, DateTimeOffset dataNascimento)
        {
            return new Usuario(nomeCompleto, email, dataNascimento);
        }

        public void AdicionarEndereco(string logradouro, string uf, string cidade, string numero, string cep, string bairro, string complemento)
        {
            Endereco = new Endereco(logradouro, uf, cidade, numero, cep, bairro, complemento);
        }

        public void Atualizar(string nomeCompleto, DateTimeOffset dataNascimento, string logradouro, string uf, string cidade, string numero, string cep, string bairro, string? complemento)
        {
            NomeCompleto = nomeCompleto;
            DataNascimento = dataNascimento;
            Endereco = new Endereco(logradouro, uf, cidade, numero, cep, bairro, complemento ?? string.Empty);
        }

        public void AtualizarFotoPerfil(string key)
        {
            UrlFotoPerfil = key;
        }
    }
}
