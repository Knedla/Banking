using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;

namespace Banking.Application.Interfaces.Services;

public interface ITransactionPolicyService<TPolicy> where TPolicy : ITransactionPolicy
{
    Task<List<TransactionPolicyResult>> EvaluatePoliciesAsync(Transaction transaction, Guid userId, CancellationToken cancellationToken = default);
}