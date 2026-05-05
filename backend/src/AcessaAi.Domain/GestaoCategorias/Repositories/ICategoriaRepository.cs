using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.Common;

namespace AcessaAi.Domain.GestaoCategorias.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<List<Categoria>> ListarAtivasAsync(CancellationToken cancellationToken);
}
