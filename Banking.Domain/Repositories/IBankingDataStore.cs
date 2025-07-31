using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Parties;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Interfaces.Entities;

namespace Banking.Domain.Repositories;

public interface IBankingDataStore
{
    List<WorkItem> WorkItems { get; }
    List<InvolvedParty> InvolvedParties { get; }
    List<Individual> Individuals { get; }
    List<Account> Accounts { get; }
    List<Transaction> Transactions { get; }

    List<T> Get<T>() where T : class, IEntity;

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

    Task SaveAsync(); // <-- flush but don’t commit

    // Nested / Savepoint Support
    Task CreateSavepointAsync();
    Task RollbackToSavepointAsync();
    Task ReleaseSavepointAsync();
}
