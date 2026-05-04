using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using ErrorOr;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities
{
    public class Estabelecimento : EntityBase
    {
        public string Nome { get; set; }    
        public Geocordenadas Geolocalizacao { get; set; }
        public Endereco Endereco { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; }

        private Estabelecimento() { }

        protected Estabelecimento(string nome, Geocordenadas geolocalizacao)
        {
            Nome = nome;
            Geolocalizacao = geolocalizacao;
        }

        public static ErrorOr<Estabelecimento> Criar(string nome, Geocordenadas geocordenadas)
        {
            var erros = new List<Error>();

            if(string.IsNullOrEmpty(nome))
                erros.Add(EstabelecimentoErros.NomeObrigatorio);

            if(geocordenadas == null)
                erros.Add(EstabelecimentoErros.GeocordenadasObrigatorio);


            if(erros.Count > 0)
                return erros;

            return new Estabelecimento(nome, geocordenadas);
        }

        public void Alterar(string nome, Geocordenadas geolocalizacao)
        {
            Nome = nome;
            Geolocalizacao = geolocalizacao;
            DataAtualizacao = DateTime.Now;
        }

        public void Desativar()
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
