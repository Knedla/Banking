using Banking.Domain.Entities.Accounts;

namespace Banking.Domain.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid accountId);
    Task<IEnumerable<Account>> GetAllAsync();
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Guid accountId);
    Task<bool> ExistsAsync(Guid accountId);
}
