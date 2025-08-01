﻿using Banking.Domain.Entities.Accounts;

namespace Banking.Domain.Repositories;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
}
