using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Extensaions;

public static class TransactionAccountDetailsExtensions
{
    public static async Task<Account> TryResolveAccount(this Application.Models.Common.TransactionAccountDetails transactionAccountDetails, IAccountRepository accountRepository)
    {
        if (transactionAccountDetails?.AccountId == Guid.Empty)
            return await accountRepository.GetByIdAsync(transactionAccountDetails.AccountId.Value);
        else if (!string.IsNullOrEmpty(transactionAccountDetails.AccountNumber))
            return await accountRepository.GetByAccountNumberAsync(transactionAccountDetails.AccountNumber);

        return null;
    }

    public static async Task<Account> TryResolveAccount(this TransactionAccountDetails transactionAccountDetails, IAccountRepository accountRepository)
    {
        if (transactionAccountDetails.Account != null)
            return transactionAccountDetails.Account;
        if (transactionAccountDetails?.AccountId == Guid.Empty)
            return await accountRepository.GetByIdAsync(transactionAccountDetails.AccountId.Value);
        else if (!string.IsNullOrEmpty(transactionAccountDetails.AccountNumber))
            return await accountRepository.GetByAccountNumberAsync(transactionAccountDetails.AccountNumber);

        return null;
    }

    public static async Task<bool> CheckIfTheAccountBelongsToTheSystem(this TransactionAccountDetails transactionAccountDetails, IAccountRepository accountRepository)
    {
        if (transactionAccountDetails.AccountId != null)
        {
            var account = await accountRepository.GetByIdAsync(transactionAccountDetails.AccountId.Value);
            transactionAccountDetails.Account = account;
        }

        return transactionAccountDetails.Account != null; // TODO: implement for real
    }
}
