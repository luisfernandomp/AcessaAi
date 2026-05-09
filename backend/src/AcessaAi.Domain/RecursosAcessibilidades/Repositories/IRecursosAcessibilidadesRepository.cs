using AcessaAi.Domain.Common;
using AcessaAi.Domain.RecursosAcessibilidades.Entities;

namespace AcessaAi.Domain.RecursosAcessibilidades.Repositories;

public interface IRecursoAcessibilidadeRepository : IRepository<RecursoAcessibilidade>
{
    Task<List<RecursoAcessibilidade>> ListarAtivasAsync(CancellationToken cancellationToken);
}
