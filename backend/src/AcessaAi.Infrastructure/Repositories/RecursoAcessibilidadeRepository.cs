using AcessaAi.Domain.RecursosAcessibilidades.Entities;
using AcessaAi.Domain.RecursosAcessibilidades.Repositories;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class RecursoAcessibilidadeRepository : Repository<RecursoAcessibilidade>, IRecursoAcessibilidadeRepository
    {
        public RecursoAcessibilidadeRepository(AcessaAiDbContext context) : base(context){}

        public async Task<List<RecursoAcessibilidade>> ListarAtivasAsync(CancellationToken cancellationToken)
            => await _dbSet.Where(c => c.Ativo).ToListAsync(cancellationToken);
    }
}
