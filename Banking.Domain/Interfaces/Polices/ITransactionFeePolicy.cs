using Banking.Domain.Entities.Transactions;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Interfaces.Polices;

public interface ITransactionFeePolicy : IPolicy
{
    Task<List<Fee>> EvaluateAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
