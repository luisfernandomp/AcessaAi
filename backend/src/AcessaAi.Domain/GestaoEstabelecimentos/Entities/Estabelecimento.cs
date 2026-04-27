using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using AcessaAi.Domain.ValueObjects;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities
{
    public class Estabelecimento : EntityBase
    {
        public string Nome { get; set; }    
        public Geocordenadas Geolocalizacao { get; set; }
        public Endereco Endereco { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

        private Estabelecimento() { }

        protected Estabelecimento(string nome, Geocordenadas geolocalizacao)
        {
            Nome = nome;
            Geolocalizacao = geolocalizacao;
        }

        public Estabelecimento CriarEstabelecimento(string nome, Geocordenadas geolocalizacao)
        {
            return new Estabelecimento(nome, geolocalizacao);
        }

        public void AtualizarEstabelecimento(string nome, Geocordenadas geolocalizacao)
        {
            Nome = nome;
            Geolocalizacao = geolocalizacao;
            DataAtualizacao = DateTime.Now;
        }

        public void ExcluirEstabelecimento()
        {
            Ativo = false;
            DataAtualizacao = DateTime.Now;
        }

        public void AdicionarAvaliacao(Avaliacao avaliacao)
        {
            Avaliacoes ??= new List<Avaliacao>();

            Avaliacoes.Add(avaliacao);
            DataAtualizacao = DateTime.Now;
        }
    }
}
