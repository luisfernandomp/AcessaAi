namespace AcessaAi.Domain.Common
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken ct);
        Task BeginTransactionAsync(CancellationToken ct);
        Task CommitTransactionAsync(CancellationToken ct);
        Task RollbackTransactionAsync(CancellationToken ct);
    }
}
