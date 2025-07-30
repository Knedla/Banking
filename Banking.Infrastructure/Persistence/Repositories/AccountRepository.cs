using Banking.Domain.Entities.Accounts;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(IBankingDataStore dataStore) : base(dataStore) { }

    public Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        var account = _dbSet.Find(s => s.AccountNumber == accountNumber);
        return Task.FromResult(account);
    }
}
