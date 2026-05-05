using AcessaAi.Application.Categorias.Dtos.Responses;
using AcessaAi.Application.Categorias.Interfaces;
using AcessaAi.Domain.GestaoCategorias.Repositories;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.Categorias.Services
{
    public class CategoriaApplicationService : ICategoriaApplicationService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaApplicationService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<ErrorOr<List<CategoriaResponse>>> ListarCategoriasAtivasAsync(CancellationToken cancellationToken)
        {
            var categorias = await _categoriaRepository.ListarAtivasAsync(cancellationToken);
            return categorias.Adapt<List<CategoriaResponse>>();
        }
    }
}
