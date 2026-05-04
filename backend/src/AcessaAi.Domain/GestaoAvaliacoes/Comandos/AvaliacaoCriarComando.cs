using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;

namespace AcessaAi.Domain.GestaoAvaliacoes.Comandos
{
    public class AvaliacaoCriarComando
    {
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
        public Usuario Usuario { get; set; }
        public Estabelecimento Estabelecimento { get; set; }
    }
}
