using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using ErrorOr;
using System.Threading;

namespace AcessaAi.Application.Avaliacoes.Interfaces
{
    public interface IAvaliacaoApplicationService
    {
        Task<ErrorOr<AvaliacaoResponse>> CriarAsync(AvaliacaoCreateRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<AvaliacaoResponse>> AtualizarAsync(AvaliacaoUpdateRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> ExcluirAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<AvaliacaoResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    }
}