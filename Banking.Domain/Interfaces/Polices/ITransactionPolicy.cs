using Banking.Domain.Entities.Transactions;
using Banking.Domain.Policies;

namespace Banking.Domain.Interfaces.Polices;

public interface ITransactionPolicy : IPolicy
{
    Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid userId, CancellationToken cancellationToken = default);
}
