using Banking.Domain.Configuration;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.Models;

namespace Banking.Infrastructure.Rules.TransactionApprovals;

public class FourEyeApprovalRule : ITransactionApprovalRule
{
    private readonly FourEyeApprovalSettings _settings;

    public FourEyeApprovalRule(FourEyeApprovalSettings settings)
    {
        _settings = settings;
    }

    public Task<ApprovalDecision> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        if (!_settings.RequiresApproval)
            return Task.FromResult(ApprovalDecision.Approve());

        if (transaction.CreatedByUserId == currentUserId)
        {
            return Task.FromResult(
                ApprovalDecision.Reject("4-eye principle violation: The approver cannot be the same as the creator."));
        }

        return Task.FromResult(ApprovalDecision.Approve());
    }

    public bool RequiresApproval(Transaction transaction) => _settings.RequiresApproval;

    public ApprovalRequirement? DescribeRequirement(Transaction transaction)
    {
        if (!_settings.RequiresApproval) return null;

        return new ApprovalRequirement
        {
            RuleName = nameof(FourEyeApprovalRule),
            RequiredRoles = new List<string> { "Approver" },
            ApprovalGroups = new List<string> { "RiskTeam" },
            MinimumApprovals = 2,
            Justification = "Transaction must be approved by a different user than the one who created it."
        };
    }
}
