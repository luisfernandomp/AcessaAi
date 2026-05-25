using AcessaAi.Domain.GestaoUsuarios.Repositories;
using AcessaAi.Domain.Usuarios.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioRepository(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
            => await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken)
            => await _userManager.FindByEmailAsync(email);

        public async Task<ErrorOr<Usuario>> CriarAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
        {
            var result = await _userManager.CreateAsync(usuario, senha);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            return usuario;
        }

        public async Task<bool> ValidarSenhaAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
            => await _userManager.CheckPasswordAsync(usuario, senha);

        public async Task<IList<string>> ObterRolesAsync(Usuario usuario, CancellationToken cancellationToken)
            => await _userManager.GetRolesAsync(usuario);

        public async Task<ErrorOr<Updated>> AtualizarFotoPerfilAsync(int id, string key, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (usuario is null)
                return Error.NotFound("Usuario.NaoEncontrado", "Usuário não encontrado.");

            usuario.AtualizarFotoPerfil(key);

            var result = await _userManager.UpdateAsync(usuario);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            return Result.Updated;
        }
    }
}
