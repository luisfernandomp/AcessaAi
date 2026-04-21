using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Identity
{
    public class AcessaAiDbContext : IdentityDbContext<ApplicationUser>
    {
        public AcessaAiDbContext(DbContextOptions<AcessaAiDbContext> options) : base(options) { }
    }
}
