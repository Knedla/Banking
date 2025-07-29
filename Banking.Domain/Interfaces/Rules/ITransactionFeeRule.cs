using Banking.Domain.Entities.Transactions;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Interfaces.Rules;

public interface ITransactionFeeRule
{
    Task<bool> AppliesToAsync(Transaction transaction);
    Task<Fee?> GetFeeAsync(Transaction transaction);
}
