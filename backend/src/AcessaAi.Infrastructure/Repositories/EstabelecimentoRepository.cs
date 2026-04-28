using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class EstabelecimentoRepository : Repository<Estabelecimento>, IEstabelecimentoRepository
    {
        public EstabelecimentoRepository(AcessaAiDbContext context) : base(context)
        {
        }

        public async Task<bool> ExisteEstabelecimentoAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Estabelecimentos.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
