using AcessaAi.Application.RecursosAcessibilidades.Dtos.Responses;
using ErrorOr;

namespace AcessaAi.Application.RecursosAcessibilidades.Interfaces;

public interface IRecursoAcessibilidadeApplicationService
{
    Task<ErrorOr<List<RecursoAcessibilidadeResponse>>> ListarRecursosAcessibilidadesAtivasAsync(CancellationToken cancellationToken);
}
