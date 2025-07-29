using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Plicies;
using Banking.Domain.Models;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

public class TransactionApprovalService : ITransactionApprovalService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionApprovalPolicy _transactionApprovalPolicy;

    public TransactionApprovalService(
        ITransactionRepository transactionRepository,
        ITransactionApprovalPolicy transactionApprovalPolicy)
    {
        _transactionRepository = transactionRepository;
        _transactionApprovalPolicy = transactionApprovalPolicy;
    }

    public async Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var approvalDecision = await _transactionApprovalPolicy.EvaluateAsync(transaction, currentUserId, cancellationToken);

        if (approvalDecision.IsApproved)
            transaction.Status = TransactionStatus.Approved;
        else
        {
            var timestamp = DateTime.UtcNow;
            transaction.RequiresApproval = true;
            transaction.ApprovalStatus = ApprovalStatus.Pending;
            if (transaction.ApprovalRequirements == null)
                transaction.ApprovalRequirements = new List<TransactionApprovalRequirement>();

            var approvalRequirements = await _transactionApprovalPolicy.GetRequirements(transaction);
            foreach (var item in approvalRequirements)
                transaction.ApprovalRequirements.Add( // TODO: add mapper
                    new TransactionApprovalRequirement()
                    {
                        Id = Guid.NewGuid(),
                        TransactionId = transaction.Id,
                        RuleName = item.RuleName,
                        RequiredRoles = item.RequiredRoles,
                        ApprovalGroups = item.ApprovalGroups,
                        MinimumApprovals = item.MinimumApprovals,
                        Justification = item.Justification,
                        CreatedAt = timestamp
                    });
        }
        await _transactionRepository.UpdateAsync(transaction);

        if (approvalDecision.IsApproved) // update balance
        {
            
        }

        return approvalDecision;
    }
}
