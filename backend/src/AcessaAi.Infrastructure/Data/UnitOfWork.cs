using AcessaAi.Domain.Common;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace AcessaAi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AcessaAiDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AcessaAiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken ct)
            => _transaction = await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct)
        {
            await _transaction!.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct)
        {
            await _transaction!.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
