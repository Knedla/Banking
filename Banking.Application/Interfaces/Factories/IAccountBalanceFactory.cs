using Banking.Application.Models.Requests;
using Banking.Domain.Entities.Accounts;

namespace Banking.Application.Interfaces.Factories;

public interface IAccountBalanceFactory
{
    Task<List<AccountBalance>> CreateInitialBalancesAsync(AccountBalanceInitializationContext context);
}
