namespace Banking.Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task SaveAsync(); // <-- flush but don’t commit

        // Nested / Savepoint Support
        Task CreateSavepointAsync();
        Task RollbackToSavepointAsync();
        Task ReleaseSavepointAsync();
    }
}
