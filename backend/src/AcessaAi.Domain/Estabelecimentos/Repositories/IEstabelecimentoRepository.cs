using AcessaAi.Domain.Common;
using AcessaAi.Domain.Estabelecimentos.Consultas;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using ErrorOr;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Repositories
{
    public interface IEstabelecimentoRepository : IRepository<Estabelecimento>
    {
        Task<bool> ExisteEstabelecimentoAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<List<Estabelecimento>>> FiltrarAsync(EstabelecimentoFiltrarConsulta consulta, CancellationToken cancellationToken);
    }
}
