using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AcessaAi.Infrastructure.Identity
{
    public class AcessaAiDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AcessaAiDbContext(DbContextOptions<AcessaAiDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Estabelecimento> Estabelecimentos => Set<Estabelecimento>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}