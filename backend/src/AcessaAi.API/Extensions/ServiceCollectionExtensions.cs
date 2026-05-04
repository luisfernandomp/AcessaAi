using AcessaAi.Application.Autenticacao.Services;
using AcessaAi.Application.Mappings;
using AcessaAi.Domain.Common;
using AcessaAi.Infrastructure.Data;
using AcessaAi.Infrastructure.Identity;
using AcessaAi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AcessaAi.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcessaAiDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScopedByConvention(
                typeof(AutenticacaoService).Assembly,
                typeof(UnitOfWork).Assembly
            );

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private static IServiceCollection AddScopedByConvention(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var concreteTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition);

                foreach (var type in concreteTypes)
                {
                    var interfaces = type.GetInterfaces()
                        .Where(i => i.IsInterface && !i.IsGenericType && i.Namespace?.StartsWith("AcessaAi") == true);

                    foreach (var @interface in interfaces)
                        services.AddScoped(@interface, type);
                }
            }
            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            TinyMapperConfig.RegisterMappings();
            return services;
        }
    }
}
