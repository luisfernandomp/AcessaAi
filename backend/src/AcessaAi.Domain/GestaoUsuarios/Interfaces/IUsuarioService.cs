using AcessaAi.Domain.Autenticacao.Entities;

namespace AcessaAi.Domain.GestaoUsuarios.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    }
}
