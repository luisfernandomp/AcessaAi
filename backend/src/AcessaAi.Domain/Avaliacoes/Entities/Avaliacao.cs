using AcessaAi.Domain.Usuarios.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using ErrorOr;

namespace AcessaAi.Domain.Avaliacoes.Entities
{
    public class Avaliacao : EntityBase
    {
        public string Comentario { get; private set; }
        public ushort QuantidadeEstrelas { get; private set; }
        public int UsuarioId { get; private set; } 
        public int EstabelecimentoId { get; private set; }
        public Usuario Usuario { get; private set; }
        public Estabelecimento Estabelecimento { get; private set; }

        private Avaliacao() { }
        private Avaliacao(string comentario, ushort estrelas, Usuario usuario, Estabelecimento estabelecimento) 
        {
            Comentario = comentario;
            QuantidadeEstrelas = estrelas;
            Usuario = usuario;
            Estabelecimento = estabelecimento;
        }

        public static ErrorOr<Avaliacao> Criar(string comentario, ushort estrelas, Usuario usuario, Estabelecimento estabelecimento)
        {
            var erros = new List<Error>();

            if(usuario == null)
                erros.Add(AvaliacaoErrors.UsuarioObrigatorio);

            if (estabelecimento == null)
                erros.Add(AvaliacaoErrors.EstabelecimentoObrigatorio);

            if (estrelas < 1 || estrelas > 5)
                erros.Add(AvaliacaoErrors.NotaForaDoIntervalo);

            if (string.IsNullOrWhiteSpace(comentario))
                erros.Add(AvaliacaoErrors.ComentarioObrigatorio);
            else if (comentario.Length > 500)
                erros.Add(AvaliacaoErrors.ComentarioMuitoLongo);
            
            if (erros.Count > 0)
                return erros;

            return new Avaliacao(comentario, estrelas, usuario, estabelecimento);
        }

        public ErrorOr<Avaliacao> Alterar(string comentario, ushort estrelas)
        {
            var erros = new List<Error>();

            if (string.IsNullOrWhiteSpace(comentario))
                erros.Add(AvaliacaoErrors.ComentarioObrigatorio);
            else if (comentario.Length > 500)
                erros.Add(AvaliacaoErrors.ComentarioMuitoLongo);
            
            if (estrelas < 1 || estrelas > 5)
                erros.Add(AvaliacaoErrors.NotaForaDoIntervalo);

            if (erros.Count > 0)
                return erros;

            Comentario = comentario;
            QuantidadeEstrelas = estrelas;
            DataAtualizacao = DateTime.UtcNow;
            return this;
        }

        public void Excluir()
        {
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
