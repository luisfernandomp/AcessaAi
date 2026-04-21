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

        }
    }
}
