using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.GestaoAvaliacoes.Repositories;
using AcessaAi.Infrastructure.Identity;

namespace AcessaAi.Infrastructure.Repositories
{
    public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(AcessaAiDbContext context) : base(context)
        {
        }
    }
}
