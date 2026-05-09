using AcessaAi.Application.RecursosAcessibilidades.Dtos.Responses;
using AcessaAi.Application.RecursosAcessibilidades.Interfaces;
using AcessaAi.Domain.RecursosAcessibilidades.Repositories;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.RecursosAcessibilidades.Services
{
    public class RecursoAcessibilidadeApplicationService : IRecursoAcessibilidadeApplicationService
    {
        private readonly IRecursoAcessibilidadeRepository _recursoAcessibilidadeRepository;

        public RecursoAcessibilidadeApplicationService(IRecursoAcessibilidadeRepository recursoAcessibilidadeRepository)
        {
            _recursoAcessibilidadeRepository = recursoAcessibilidadeRepository;
        }

        public async Task<ErrorOr<List<RecursoAcessibilidadeResponse>>> ListarRecursosAcessibilidadesAtivasAsync(CancellationToken cancellationToken)
        {
            var recursos = await _recursoAcessibilidadeRepository.ListarAtivasAsync(cancellationToken);
            return recursos.Adapt<List<RecursoAcessibilidadeResponse>>();
        }
    }
}
