using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.GestaoCategorias.Repositories;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AcessaAiDbContext context) : base(context){}

        public async Task<List<Categoria>> ListarAtivasAsync(CancellationToken cancellationToken)
            => await _dbSet.Where(c => c.Ativo).ToListAsync(cancellationToken);
    }
}
