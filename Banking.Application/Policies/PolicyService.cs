using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;

namespace Banking.Application.Policies;

public class PolicyService<TPolicy> : IPolicyService<TPolicy> where TPolicy : ITransactionPolicy
{
    private readonly IEnumerable<TPolicy> _policies;

    public PolicyService(IEnumerable<TPolicy> policies)
    {
        _policies = policies;
    }

    public async Task<List<TransactionPolicyResult>> EvaluatePoliciesAsync(
        Transaction transaction,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var results = new List<TransactionPolicyResult>();

        foreach (var policy in _policies)
        {
            var result = await policy.EvaluateAsync(transaction, userId, cancellationToken);
            results.Add(result);
        }

        return results;
    }
}