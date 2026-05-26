using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using ErrorOr;

namespace AcessaAi.Application.Estabelecimentos.Interfaces
{
    public interface IEstabelecimentoApplicationService
    {
        Task<ErrorOr<EstabelecimentoListarResponse>> CriarAsync(EstabelecimentoCriarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<EstabelecimentoListarResponse>> AtualizarAsync(int id, EstabelecimentoAtualizarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> ExcluirAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<EstabelecimentoListarResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> SubirImagemAsync(int id, EstabelecimentoImagemRequest request, CancellationToken cancellationToken);
         Task<ErrorOr<IEnumerable<EstabelecimentoListarResponse>>> FiltrarAsync(
            EstabelecimentoFiltrarRequest request,
            CancellationToken cancellationToken);
    }
}
