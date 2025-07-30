using Banking.Domain.Entities.Transactions;
using Banking.Domain.Models;

namespace Banking.Application.Interfaces.Services;

public interface ITransactionApprovalService
{
    Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
    Task<ApprovalDecision> ApproveWithRelatedTransactionsAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
}
