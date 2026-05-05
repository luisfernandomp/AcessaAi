using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using ErrorOr;

namespace AcessaAi.Application.Estabelecimentos.Interfaces
{
    public interface IEstabelecimentoApplicationService
    {
        Task<ErrorOr<EstabelecimentoResponse>> CriarAsync(EstabelecimentoCriarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<EstabelecimentoResponse>> AtualizarAsync(EstabelecimentoAtualizarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> ExcluirAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<EstabelecimentoResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    }
}
