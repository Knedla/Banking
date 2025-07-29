using Banking.Domain.Entities.Transactions;
using Banking.Domain.Models;

namespace Banking.Domain.Interfaces.Plicies;

public interface ITransactionApprovalPolicy : IPolicy
{
    Task<ApprovalDecision> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
    Task<List<ApprovalRequirement>> GetRequirements(Transaction transaction, CancellationToken cancellationToken = default);
}
