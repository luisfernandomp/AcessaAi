using AcessaAi.Application.Dtos;
using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Dtos.Responses;

namespace AcessaAi.Application.Usuarios.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task<BaseResponse<UsuarioResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
        Task<BaseResponse<UsuarioResponse>> CadastrarAsync(UsuariosCadastrarRequest request, CancellationToken cancellationToken);
    }
}
