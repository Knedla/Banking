using Banking.Application.Interfaces.Factories;
using Banking.Application.Models.Requests;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Enumerations;

namespace Banking.Infrastructure.Services;

public class AccountBalanceFactory : IAccountBalanceFactory
{
    public async Task<List<AccountBalance>> CreateInitialBalancesAsync(AccountBalanceInitializationContext context)
    {
        var balances = new List<AccountBalance>();

        switch (context.AccountType)
        {
            case AccountType.Standard:
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "EUR", Balance = 0, AvailableBalance = 0 });
                break;

            case AccountType.Checking:
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "EUR", Balance = 0, AvailableBalance = 0 });
                break;

            case AccountType.Savings:
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "EUR", Balance = 0, AvailableBalance = 0 });
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "USD", Balance = 0, AvailableBalance = 0 });
                break;

            case AccountType.Loan:
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "EUR", Balance = 0, AvailableBalance = 0 });
                break;

            default:
                balances.Add(new AccountBalance { AccountId = context.AccountId, CurrencyCode = "EUR", Balance = 0, AvailableBalance = 0 });
                break;
        }

        return balances;
    }
}
