using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.GestaoAvaliacoes.Comandos;
using AcessaAi.Domain.GestaoAvaliacoes.Interfaces;
using AcessaAi.Domain.GestaoAvaliacoes.Repositories;
using ErrorOr;

namespace AcessaAi.Domain.GestaoAvaliacoes.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
        }

        public async Task<ErrorOr<Avaliacao>> AtualizarAsync(AvaliacaoAtualizarComando comando, CancellationToken cancellationToken)
        {
            var avaliacaoResult = await ObterPorIdAsync(comando.Id, cancellationToken);

            if (avaliacaoResult.IsError)
                return avaliacaoResult.Errors;

            var avaliacao = avaliacaoResult.Value;

            if(avaliacao == null)
            {
                throw new Exception("Avaliação não encontrada.");
            }

            avaliacao.Alterar(comando.Comentario, comando.Estrelas);

            await _avaliacaoRepository.UpdateAsync(avaliacao, cancellationToken);

            return avaliacao;
        }

        public async Task<ErrorOr<Avaliacao>> CriarAsync(AvaliacaoCriarComando comando, CancellationToken cancellationToken)
        {
             var avaliacaoResult = Avaliacao.Criar(comando.Comentario, comando.Estrelas, comando.UsuarioId, comando.EstabelecimentoId);

            if (avaliacaoResult.IsError)
                return avaliacaoResult.Errors;

            var avaliacao = avaliacaoResult.Value;

            await _avaliacaoRepository.AddAsync(avaliacao, cancellationToken);
            return avaliacao;
        }

        public async Task<ErrorOr<bool>> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            var avaliacaoResult = await ObterPorIdAsync(id, cancellationToken);
            
            if(avaliacaoResult.IsError)
                return avaliacaoResult.Errors;

            var avaliacao = avaliacaoResult.Value;
        
            await _avaliacaoRepository.DeleteAsync(avaliacao, cancellationToken);
            return true;
        }

        public async Task<ErrorOr<Avaliacao?>> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id, cancellationToken);
            return avaliacao;
        }
    }
}
