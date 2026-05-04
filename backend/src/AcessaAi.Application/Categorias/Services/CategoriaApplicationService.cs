using AcessaAi.Application.Categorias.Dtos.Responses;
using AcessaAi.Application.Categorias.Interfaces;
using AcessaAi.Domain.GestaoCategorias.Repositories;
using ErrorOr;

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
            var consulta = await _categoriaRepository.ListarAtivasAsync(cancellationToken);

            return consulta
                .Select(c => new CategoriaResponse
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    Icone = c.Icone
                })
                .ToList();
        }
    }
}