using AcessaAi.Application.Categorias.Dtos.Responses;
using ErrorOr;

namespace AcessaAi.Application.Categorias.Interfaces;

public interface ICategoriaApplicationService
{
    Task<ErrorOr<List<CategoriaResponse>>> ListarCategoriasAtivasAsync(CancellationToken cancellationToken);
}
