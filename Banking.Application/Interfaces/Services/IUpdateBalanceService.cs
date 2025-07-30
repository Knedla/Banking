using Banking.Domain.Entities.Transactions;

namespace Banking.Application.Interfaces.Services;

public interface IUpdateBalanceService
{
    Task UpdateBalanceAsync(Transaction transaction);
    Task UpdateBalanceAndRelatedTransactionsAsync(Transaction transaction);
}
