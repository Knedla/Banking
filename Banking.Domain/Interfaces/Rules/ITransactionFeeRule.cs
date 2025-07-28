using Banking.Domain.Entities;
using Banking.Domain.Entities.Transactions;

namespace Banking.Domain.Interfaces.Rules;

public interface ITransactionFeeRule
{
    Task<bool> AppliesToAsync(Transaction transaction);
    Task<Fee?> GetFeeAsync(Transaction transaction);
}
