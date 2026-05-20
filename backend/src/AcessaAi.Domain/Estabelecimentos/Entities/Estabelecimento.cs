using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using AcessaAi.Domain.RecursosAcessibilidades.Entities;
using ErrorOr;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities
{
    public class Estabelecimento : EntityBase
    {
        public string Nome { get; private set; }    
        public Geocordenadas Geolocalizacao { get; private set; }
        public Endereco Endereco { get; private set; }
        public double MediaEstrelas { get; private set; }
        public bool CadastradoRecente { get; private set; }
        public ICollection<EstabelecimentoFoto> Fotos { get; private set; }
        public ICollection<Avaliacao> Avaliacoes { get; private set; }
        public ICollection<RecursoAcessibilidade> RecursosAcessibilidade { get; private set; }

        private Estabelecimento() { }

        protected Estabelecimento(string nome, Geocordenadas geolocalizacao, Endereco endereco)
        {
            Nome = nome;
            Geolocalizacao = geolocalizacao;
            Endereco = endereco;
            CadastradoRecente = true;
            MediaEstrelas = 0;
            Fotos = new List<EstabelecimentoFoto>();
            Avaliacoes = new List<Avaliacao>();
            RecursosAcessibilidade = new List<RecursoAcessibilidade>();
        }

        private static (List<Error> erros, bool hasError) Validar(string nome, Geocordenadas geocordenadas)
        {
            var erros = new List<Error>();

            if(string.IsNullOrEmpty(nome))
                erros.Add(EstabelecimentoErros.NomeObrigatorio);

            if(geocordenadas == null)
                erros.Add(EstabelecimentoErros.GeocordenadasObrigatorio);

            return (erros, erros.Any());
        }

        public static ErrorOr<Estabelecimento> Criar(string nome, Geocordenadas geocordenadas, Endereco endereco)
        {
            var (erros, hasError) = Validar(nome, geocordenadas);

            if(hasError)
                return erros;

            return new Estabelecimento(nome, geocordenadas, endereco);
        }

        public ErrorOr<Estabelecimento> Alterar(string nome, Geocordenadas geolocalizacao)
        {
            var (erros, hasError) = Validar(nome, geolocalizacao);
            if(hasError)
                return erros;

            Nome = nome;
            Geolocalizacao = geolocalizacao;
            DataAtualizacao = DateTime.UtcNow;

            return this;
        }

        public void Desativar()
        {
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }

        public ErrorOr<Estabelecimento> AdicionarAvaliacao(Avaliacao avaliacao)
        {
            var erros = new List<Error>();
            
            Avaliacoes ??= new List<Avaliacao>();

            if(avaliacao.QuantidadeEstrelas < 0 || avaliacao.QuantidadeEstrelas > 5)
                erros.Add(EstabelecimentoErros.MediaEntre0e5);

            if(erros.Any())
                return erros;   

            Avaliacoes.Add(avaliacao);
            DataAtualizacao = DateTime.UtcNow;
            AtualizarMediaEstrelas();

            return this;
        }

        private void AtualizarMediaEstrelas()
        {
            if (Avaliacoes.Any())
            {
                var media = Avaliacoes.Average(a => a.QuantidadeEstrelas);
                MediaEstrelas = media;
            }
        }

        public void AdicionarImagem(string url, bool isCapa = false)
        {
            Fotos ??= new List<EstabelecimentoFoto>();
            var ehCapa = isCapa || !Fotos.Any();
            Fotos.Add(new EstabelecimentoFoto(url, ehCapa));
            DataAtualizacao = DateTime.UtcNow;
        }

        public void AdicionarRecursoAcessibilidade(RecursoAcessibilidade recurso)
        {
            RecursosAcessibilidade ??= new List<RecursoAcessibilidade>();
            RecursosAcessibilidade.Add(recurso);
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
