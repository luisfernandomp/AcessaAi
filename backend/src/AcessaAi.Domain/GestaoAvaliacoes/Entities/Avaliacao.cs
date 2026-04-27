using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Entities;

namespace AcessaAi.Domain.Avaliacoes.Entities
{
    public class Avaliacao : EntityBase
    {
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
        public Usuario Usuario { get; set; }

        protected Avaliacao(string comentario, ushort estrelas) 
        {
            Comentario = comentario;
            Estrelas = estrelas;
        }

        public Avaliacao CriarAvaliacao(string comentario, ushort estrelas)
        {
            return new Avaliacao(comentario, estrelas);
        }

        public void AtualizarAvaliacao(string comentario, ushort estrelas)
        {
            Comentario = comentario;
            Estrelas = estrelas;
            DataAtualizacao = DateTime.Now;
        }

        public void ExcluirAvaliacao()
        {
            Ativo = false;
            DataAtualizacao = DateTime.Now;
        }
    }
}
