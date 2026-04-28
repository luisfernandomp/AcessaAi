using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.Dtos;

namespace AcessaAi.Application.Avaliacoes.Interfaces
{
    public interface IAvaliacaoApplicationService
    {
        Task<BaseResponse<AvaliacaoResponse>> CriarAsync(AvaliacaoCreateRequest request, CancellationToken cancellationToken);
        Task<BaseResponse<AvaliacaoResponse>> AtualizarAsync(AvaliacaoUpdateRequest request, CancellationToken cancellationToken);
        Task<BaseResponse> ExcluirAsync(int id, CancellationToken cancellationToken);
        Task<BaseResponse<AvaliacaoResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    }
}
