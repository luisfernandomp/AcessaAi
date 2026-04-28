using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using ErrorOr;

namespace AcessaAi.Domain.Avaliacoes.Entities
{
    public class Avaliacao : EntityBase
    {
        public string Comentario { get; private set; }
        public ushort Estrelas { get; private set; }
        public int UsuarioId { get; private set; } 
        public int EstabelecimentoId { get; private set; }
        public Usuario Usuario { get; private set; }
        public Estabelecimento Estabelecimento { get; private set; }

        private Avaliacao() { }
        private Avaliacao(string comentario, ushort estrelas, int usuarioId, int estabelecimentoId) 
        {
            Comentario = comentario;
            Estrelas = estrelas;
            UsuarioId = usuarioId;
            EstabelecimentoId = estabelecimentoId;
        }

        public static ErrorOr<Avaliacao> Criar(string comentario, ushort estrelas, int usuarioId, int estabelecimentoId)
        {
            var erros = new List<Error>();

            if(usuarioId <= 0)
                erros.Add(AvaliacaoErrors.UsuarioIdObrigatorio);

            if (estabelecimentoId <= 0)
                erros.Add(AvaliacaoErrors.EstabelecimentoIdObrigatorio);

            if (estrelas < 1 || estrelas > 5)
                erros.Add(AvaliacaoErrors.NotaForaDoIntervalo);

            if (string.IsNullOrWhiteSpace(comentario))
                erros.Add(AvaliacaoErrors.ComentarioObrigatorio);
            else if (comentario.Length > 500)
                erros.Add(AvaliacaoErrors.ComentarioMuitoLongo);
            
            if (erros.Count > 0)
                return erros;

            return new Avaliacao(comentario, estrelas, usuarioId, estabelecimentoId);
        }

        public void Alterar(string comentario, ushort estrelas)
        {
            Comentario = comentario;
            Estrelas = estrelas;
            DataAtualizacao = DateTime.Now;
        }

        public void Excluir()
        {
            Ativo = false;
            DataAtualizacao = DateTime.Now;
        }
    }
}
