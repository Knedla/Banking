using Banking.Domain.Entities.Transactions;
using Banking.Domain.Models;

namespace Banking.Domain.Interfaces.Rules;

public interface ITransactionApprovalRule
{
    Task<ApprovalDecision> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
    bool RequiresApproval(Transaction transaction);
    ApprovalRequirement? DescribeRequirement(Transaction transaction);
}