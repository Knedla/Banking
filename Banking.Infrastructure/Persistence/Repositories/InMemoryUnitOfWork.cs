using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Repositories;

public class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly IBankingDataStore _dataStore;

    public InMemoryUnitOfWork(IBankingDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task BeginTransactionAsync() => _dataStore.BeginTransactionAsync();
    public Task CommitAsync() => _dataStore.CommitAsync();
    public Task RollbackAsync() => _dataStore.RollbackAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public Task SaveAsync() => _dataStore.BeginTransactionAsync();
    public Task CreateSavepointAsync() => _dataStore.CreateSavepointAsync();
    public Task RollbackToSavepointAsync() => _dataStore.RollbackToSavepointAsync();
    public Task ReleaseSavepointAsync() => _dataStore.ReleaseSavepointAsync();
}
