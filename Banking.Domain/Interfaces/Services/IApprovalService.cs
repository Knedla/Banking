using Banking.Domain.Entities.Transactions;
using Banking.Domain.Models;

namespace Banking.Domain.Interfaces.Services;

public interface IApprovalService
{
    Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
    Task<bool> CheckIfApprovalRequiredAsync(Transaction transaction);
    Task<List<ApprovalRequirement>> GetApprovalRequirementsAsync(Transaction transaction);
}