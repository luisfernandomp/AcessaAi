using AcessaAi.Domain.Entities;

namespace AcessaAi.Domain.Avaliacoes.Entities
{
    public class Avaliacao : EntityBase
    {
        public string Comentario { get; set; }
        public int Estrelas { get; set; }

        protected Avaliacao(string comentario, int estrelas) 
        {
            Comentario = comentario;
            Estrelas = estrelas;
        }

        public Avaliacao CriarAvaliacao(string comentario, int estrelas)
        {
            return new Avaliacao(comentario, estrelas);
        }

        public void AtualizarAvaliacao(string comentario, int estrelas)
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
