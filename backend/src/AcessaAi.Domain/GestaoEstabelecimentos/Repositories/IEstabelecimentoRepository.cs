using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Repositories
{
    public interface IEstabelecimentoRepository : IRepository<Estabelecimento>
    {
        Task<bool> ExisteEstabelecimentoAsync(int id, CancellationToken cancellationToken);
    }
}
