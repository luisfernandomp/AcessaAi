using AcessaAi.Application.Autenticacao.Dtos;
using AcessaAi.Application.Autenticacao.Interfaces;
using AcessaAi.Domain.GestaoUsuarios.Repositories;
using ErrorOr;
using System.Security.Claims;

namespace AcessaAi.Application.Autenticacao.Services
{
    public class AutenticacaoApplicationService : IAutenticacaoApplicationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public AutenticacaoApplicationService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<ErrorOr<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email, cancellationToken);

            if (usuario is null)
                return Error.Unauthorized("Auth.CredenciaisInvalidas", "Email ou senha inválidos.");

            var senhaValida = await _usuarioRepository.ValidarSenhaAsync(usuario, request.Senha, cancellationToken);

            if (!senhaValida)
                return Error.Unauthorized("Auth.CredenciaisInvalidas", "Email ou senha inválidos.");

            var roles = await _usuarioRepository.ObterRolesAsync(usuario, cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.UserName!),
                new(ClaimTypes.Email, usuario.Email!),
                new("jti", Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = _tokenService.GenerateAccessToken(claims);

            return new LoginResponse
            {
                Token = token,
                IdUsuario = usuario.Id,
                NomeUsuario = usuario.UserName!,
                ExpiraEm = _tokenService.GetExpirationInMinutes()
            };
        }
    }
}
