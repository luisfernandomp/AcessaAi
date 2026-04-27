using AcessaAi.Application.Autenticacao.Interfaces;
using AcessaAi.Application.Autenticacao.Services;
using AcessaAi.Application.Mappings;
using AcessaAi.Application.Usuarios.Interfaces;
using AcessaAi.Application.Usuarios.Services;
using AcessaAi.Domain.Autenticacao.Interfaces;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcessaAiDbContext>(opt => 
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            TinyMapperConfig.RegisterMappings();
            return services;
        }
    }
}