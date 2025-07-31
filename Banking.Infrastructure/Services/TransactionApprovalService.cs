using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Models;

namespace Banking.Application.Services;

public class TransactionApprovalService : ITransactionApprovalService
{
    private readonly ITransactionApprovalPolicy _transactionApprovalPolicy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ITransactionService _transactionService;
    private readonly ITransactionStateValidator _transactionStateValidator;

    public TransactionApprovalService(
        ITransactionApprovalPolicy transactionApprovalPolicy,
        IDomainEventDispatcher domainEventDispatcher,
        ITransactionService transactionService,
        ITransactionStateValidator transactionStateValidator)
    {
        _transactionApprovalPolicy = transactionApprovalPolicy;
        _domainEventDispatcher = domainEventDispatcher;
        _transactionService = transactionService;
        _transactionStateValidator = transactionStateValidator;
    }

    public async Task<List<ApprovalDecision>> ApproveWithRelatedTransactionsAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var approvalDecisions = new List<ApprovalDecision>();

        var approvalDecision = await ApproveAsync(transaction, currentUserId, cancellationToken);
        approvalDecisions.Add(approvalDecision);
        var endTransactionStatuses = _transactionStateValidator.GetEndTransactionStatuses();

        foreach (var item in transaction.RelatedTransactions.Where(s => !endTransactionStatuses.Contains(s.Status)))
        {
            approvalDecision = await ApproveAsync(item, currentUserId, cancellationToken);
            approvalDecisions.Add(approvalDecision);
        }

        if (approvalDecisions.All(s => s.IsApproved))
            await _domainEventDispatcher.RaiseAsync(new TransactionApprovedEvent(
                transaction.Id,
                transaction.TransactionInitializedById ?? Guid.Empty // InvolvedPartyId prop should be removed from IDomainEvent, eventually
            ));

        return approvalDecisions;
    }

    public async Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var approvalDecision = await _transactionApprovalPolicy.EvaluateAsync(transaction, currentUserId, cancellationToken);

        if (approvalDecision.IsApproved)
            await _transactionService.ChangeStatusAsync(transaction, TransactionStatus.Approved, currentUserId, cancellationToken); // ovo ce da se trigeruje na svaku transakciju.... 
        else
        {
            var timestamp = DateTime.UtcNow;
            transaction.RequiresApproval = true;
            transaction.ApprovalStatus = ApprovalStatus.Pending;
            if (transaction.ApprovalRequirements == null)
                transaction.ApprovalRequirements = new List<TransactionApprovalRequirement>();

            var approvalRequirements = await _transactionApprovalPolicy.GetRequirements(transaction);
            foreach (var item in approvalRequirements)
                transaction.ApprovalRequirements.Add(
                    new TransactionApprovalRequirement() // TODO: add mapper
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
            await _transactionService.UpdateAsync(transaction, currentUserId, cancellationToken);
        }

        return approvalDecision;
    }
}
