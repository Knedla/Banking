using Banking.Domain.Entities.Accounts;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = new();

    public Task<Account?> GetByIdAsync(Guid accountId)
    {
        return Task.FromResult(_accounts.FirstOrDefault(a => a.Id == accountId));
    }

    public Task<IEnumerable<Account>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Account>>(_accounts);
    }

    public Task AddAsync(Account account)
    {
        _accounts.Add(account);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Account account)
    {
        var existing = _accounts.FirstOrDefault(a => a.Id == account.Id);
        if (existing != null)
        {
            _accounts.Remove(existing);
            _accounts.Add(account);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);
        if (account != null)
            _accounts.Remove(account);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid accountId)
    {
        return Task.FromResult(_accounts.Any(a => a.Id == accountId));
    }
}
