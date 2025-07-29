using Banking.Domain.Entities.Transactions;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(IBankingDataStore dataStore) : base(dataStore) { }
}
