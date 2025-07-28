using Banking.Domain.Entities.Accounts;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Stores;

public class InMemoryBankingDataStore : IBankingDataStore
{
    //private List<Customer> _customers = new();
    //private List<AccountType> _accountTypes = new();
    private List<Account> _accounts = new();

    //private List<Customer> _customersBackup;
    //private List<AccountType> _accountTypesBackup;
    private List<Account> _accountsBackup;

    //public List<Customer> Customers => _customers;
    //public List<AccountType> AccountTypes => _accountTypesBackup;
    public List<Account> Accounts => _accounts;

    public Task BeginTransactionAsync()
    {
        _accountsBackup = _accounts.Select(a => new Account
        {
            Id = a.Id,
            AccountNumber = a.AccountNumber,
            //Balance = a.Balance
        }).ToList();

        //_accountTypesBackup = new List<AccountType>(_accountTypes.Select(t => t.Clone()));
        //_customersBackup = new List<Customer>(_customers.Select(l => l.Clone()));

        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        //_customersBackup = null;
        //_accountTypesBackup = null;
        _accountsBackup = null;
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        //_customers = _customersBackup;
        //_accountTypes = _accountTypesBackup;
        _accounts = _accountsBackup;

        return Task.CompletedTask;
    }

    // TODO: implement
    public Task SaveAsync() => throw new NotImplementedException();
    public Task CreateSavepointAsync() => throw new NotImplementedException();
    public Task RollbackToSavepointAsync() => throw new NotImplementedException();
    public Task ReleaseSavepointAsync() => throw new NotImplementedException();
}
