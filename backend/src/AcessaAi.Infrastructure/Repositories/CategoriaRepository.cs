using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.GestaoCategorias.Consultas;
using AcessaAi.Domain.GestaoCategorias.Repositories;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AcessaAiDbContext context) : base(context)
        {
        }

        public async Task<List<CategoriaListarConsulta>> ListarAtivasAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Where(c => c.Ativo)
                .Select(c => new CategoriaListarConsulta
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    Icone = c.Icone
                })
                .ToListAsync(cancellationToken);
        }
    }
}