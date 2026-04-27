using System.Security.Claims;

namespace AcessaAi.Domain.Autenticacao.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        int GetExpirationInMinutes();
    }
}
