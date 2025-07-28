using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Services;
using Banking.Domain.Models;
using Banking.Domain.Policies;

namespace Banking.Application.Services.Approval;

public class ApprovalService : IApprovalService
{
    private readonly TransactionApprovalPolicy _policy;

    public ApprovalService(TransactionApprovalPolicy policy)
    {
        _policy = policy;
    }

    public async Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        return await _policy.EvaluateAsync(transaction, currentUserId, cancellationToken);
    }

    public Task<bool> CheckIfApprovalRequiredAsync(Transaction transaction)
    {
        return Task.FromResult(_policy.RequiresApproval(transaction));
    }

    public Task<List<ApprovalRequirement>> GetApprovalRequirementsAsync(Transaction transaction)
    {
        return Task.FromResult(_policy.GetRequirements(transaction));
    }
}
