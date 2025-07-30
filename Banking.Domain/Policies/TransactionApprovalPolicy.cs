using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.Models;

namespace Banking.Domain.Policies;

public class TransactionApprovalPolicy : ITransactionApprovalPolicy
{
    private readonly IEnumerable<ITransactionApprovalRule> _rules;

    public TransactionApprovalPolicy(IEnumerable<ITransactionApprovalRule> rules)
    {
        _rules = rules;
    }

    public async Task<ApprovalDecision> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        foreach (var rule in _rules)
        {
            var decision = await rule.EvaluateAsync(transaction, currentUserId, cancellationToken);
            if (!decision.IsApproved)
                return decision; // Stop on first failure
        }

        return ApprovalDecision.Approve();
    }

    public Task<List<ApprovalRequirement>> GetRequirements(Transaction transaction, CancellationToken cancellationToken = default)
    {
        var result = _rules
            .Where(rule => rule.RequiresApproval(transaction))
            .Select(rule => rule.DescribeRequirement(transaction))
            .Where(r => r != null)
            .Cast<ApprovalRequirement>()
            .ToList();

        return Task.FromResult(result);
    }
}
