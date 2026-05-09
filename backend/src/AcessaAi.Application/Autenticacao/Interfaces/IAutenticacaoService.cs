using AcessaAi.Application.Autenticacao.Dtos;
using ErrorOr;

namespace AcessaAi.Application.Autenticacao.Interfaces
{
    public interface IAutenticacaoApplicationService
    {
        Task<ErrorOr<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
