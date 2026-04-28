using AcessaAi.Domain.Common;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AcessaAiDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AcessaAiDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }
    }
}
