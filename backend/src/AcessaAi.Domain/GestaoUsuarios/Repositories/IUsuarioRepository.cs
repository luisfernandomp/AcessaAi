using AcessaAi.Domain.Autenticacao.Entities;
using ErrorOr;

namespace AcessaAi.Domain.GestaoUsuarios.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken);
        Task<ErrorOr<Usuario>> CriarAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<bool> ValidarSenhaAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<IList<string>> ObterRolesAsync(Usuario usuario, CancellationToken cancellationToken);
    }
}
