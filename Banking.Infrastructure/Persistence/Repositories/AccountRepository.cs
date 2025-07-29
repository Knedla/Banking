using Banking.Domain.Entities.Accounts;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(IBankingDataStore dataStore) : base(dataStore) { }
}
