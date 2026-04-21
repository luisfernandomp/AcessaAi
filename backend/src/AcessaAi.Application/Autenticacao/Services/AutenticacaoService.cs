using AcessaAi.Application.Autenticacao.Dtos;
using AcessaAi.Application.Autenticacao.Interfaces;
using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Autenticacao.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AcessaAi.Application.Autenticacao.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ILogger<AutenticacaoService> _logger;
        private readonly ITokenService _tokenService;

        public AutenticacaoService(UserManager<Usuario> userManager, 
            SignInManager<Usuario> signInManager,
            ITokenService tokenService,
            ILogger<AutenticacaoService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }   

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
			try
			{
                var user = await _userManager.FindByEmailAsync(request.Email);

                if(user == null)
                {

                }

                var senhaValida = await _signInManager.CheckPasswordSignInAsync(user, request.Senha, false);

                if (!senhaValida.Succeeded)
                {

                }

                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = _tokenService.GenerateAccessToken(claims);

                return new LoginResponse
                {
                    Token = token,
                    IdUsuario = user.Id,
                    NomeUsuario = user.UserName,
                    ExpiraEm = _tokenService.GetExpirationInMinutes()
                };

            }
			catch (Exception ex)
			{
				throw;
			}
        }
    }
}
