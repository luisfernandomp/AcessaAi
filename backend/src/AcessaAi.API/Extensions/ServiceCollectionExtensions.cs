using AcessaAi.Application.Autenticacao.Interfaces;
using AcessaAi.Application.Autenticacao.Services;
using AcessaAi.Infrastructure.Autenticacao;
using AcessaAi.Application.Avaliacoes.Interfaces;
using AcessaAi.Application.Avaliacoes.Services;
using AcessaAi.Application.Categorias.Interfaces;
using AcessaAi.Application.Categorias.Services;
using AcessaAi.Application.Estabelecimentos.Interfaces;
using AcessaAi.Application.Estabelecimentos.Services;
using AcessaAi.Application.Mappings;
using AcessaAi.Application.Usuarios.Interfaces;
using AcessaAi.Application.Usuarios.Services;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Repositories;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Domain.GestaoCategorias.Repositories;
using AcessaAi.Domain.GestaoUsuarios.Repositories;
using AcessaAi.Infrastructure.Data;
using AcessaAi.Infrastructure.Identity;
using AcessaAi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcessaAiDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Infrastructure
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
            services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            // Application
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();
            services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddScoped<IEstabelecimentoApplicationService, EstabelecimentoApplicationService>();
            services.AddScoped<IAvaliacaoApplicationService, AvaliacaoApplicationService>();
            services.AddScoped<ICategoriaApplicationService, CategoriaApplicationService>();

            MapsterConfig.RegisterMappings();
        }
    }
}
