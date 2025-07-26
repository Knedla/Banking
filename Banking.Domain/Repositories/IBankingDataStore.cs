using Banking.Domain.Entities;

namespace Banking.Domain.Repositories
{
    public interface IBankingDataStore
    {
        List<Customer> Customers { get; }
        List<AccountType> AccountTypes { get; }
        List<Account> Accounts { get; }

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
