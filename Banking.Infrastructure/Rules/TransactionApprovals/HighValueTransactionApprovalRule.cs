using Banking.Domain.Configuration;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.Models;

namespace Banking.Infrastructure.Rules.TransactionApprovals;

public class HighValueTransactionApprovalRule : ITransactionApprovalRule
{
    private readonly HighValueApprovalSettings _settings;

    // there should be default currency as well
    // if there is no currency in config do conversion and compare it with default currency threshold
    public HighValueTransactionApprovalRule(HighValueApprovalSettings settings) 
    {
        _settings = settings;
    }

    public Task<ApprovalDecision> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        if (RequiresApproval(transaction))
        {
            return Task.FromResult(ApprovalDecision.Reject(
                $"High-value transaction requires approval. Amount: {transaction.CurrencyAmount.Amount} {transaction.CurrencyAmount.Currency}"
            ));
        }

        return Task.FromResult(ApprovalDecision.Approve());
    }

    public bool RequiresApproval(Transaction transaction)
    {
        if (string.IsNullOrWhiteSpace(transaction.CurrencyAmount.Currency)) return false;

        if (_settings.Thresholds.TryGetValue(transaction.CurrencyAmount.Currency, out var threshold))
        {
            return transaction.CurrencyAmount.Amount > threshold;
        }

        return false;
    }

    public ApprovalRequirement? DescribeRequirement(Transaction transaction)
    {
        if (!RequiresApproval(transaction)) return null;

        _settings.Thresholds.TryGetValue(transaction.CurrencyAmount.Currency, out var threshold);

        return new ApprovalRequirement
        {
            RuleName = nameof(HighValueTransactionApprovalRule),
            RequiredRoles = new List<string> { "SeniorApprover" },
            ApprovalGroups = new List<string> { "ComplianceTeam" },
            MinimumApprovals = 1,
            Justification = $"Transaction exceeds threshold for {transaction.CurrencyAmount.Currency}: {threshold}"
        };
    }
}
