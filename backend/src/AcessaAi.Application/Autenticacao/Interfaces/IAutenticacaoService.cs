using AcessaAi.Application.Autenticacao.Dtos;

namespace AcessaAi.Application.Autenticacao.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
