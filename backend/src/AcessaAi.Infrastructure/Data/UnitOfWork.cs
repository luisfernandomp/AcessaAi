using AcessaAi.Domain.Common;
using AcessaAi.Infrastructure.Identity;

namespace AcessaAi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AcessaAiDbContext _context;

        public UnitOfWork(AcessaAiDbContext context)
        {
            _context = context;
        }

        public Task BeginTransactionAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);    
        }

        public Task CommitTransactionAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransactionAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
