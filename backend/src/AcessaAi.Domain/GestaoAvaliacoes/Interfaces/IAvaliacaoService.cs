using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.GestaoAvaliacoes.Comandos;
using ErrorOr;

namespace AcessaAi.Domain.GestaoAvaliacoes.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<ErrorOr<Avaliacao>> CriarAsync(AvaliacaoCriarComando comando, CancellationToken cancellationToken);
        Task<ErrorOr<Avaliacao>> AtualizarAsync(AvaliacaoAtualizarComando comando, CancellationToken cancellationToken);
        Task<ErrorOr<bool>> ExcluirAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<Avaliacao?>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    }
}
