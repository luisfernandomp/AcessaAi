namespace AcessaAi.Domain.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
