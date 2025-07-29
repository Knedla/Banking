using Banking.Domain.Entities.Transactions;
using Banking.Domain.Policies;

namespace Banking.Domain.Interfaces.Plicies;

public interface IDepositPolicy : IPolicy
{
    Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
}
