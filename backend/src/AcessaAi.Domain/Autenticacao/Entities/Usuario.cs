using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AcessaAi.Domain.Autenticacao.Entities
{
    public class Usuario : IdentityUser<int>
    {
        public string Nome { get; set; }
        public string Email { get; set; }   
        public string Senha { get; set; }
        public Endereco Endereco { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public List<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();  
    }
}
