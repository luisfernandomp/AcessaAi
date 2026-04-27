using AcessaAi.Application.Dtos;
using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Dtos.Responses;

namespace AcessaAi.Application.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        Task<BaseResponse<UsuarioResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<BaseResponse<UsuarioResponse>> CadastrarAsync(UsuariosCadastrarRequest request, CancellationToken cancellationToken);
    }
}
