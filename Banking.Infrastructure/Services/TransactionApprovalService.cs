using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Models;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

public class TransactionApprovalService : ITransactionApprovalService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionApprovalPolicy _transactionApprovalPolicy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IStateTransitionValidator<TransactionStatus> _stateTransitionValidator;
    private readonly IUpdateBalanceService _updateBalanceService;

    public TransactionApprovalService(
        ITransactionRepository transactionRepository,
        ITransactionApprovalPolicy transactionApprovalPolicy,
        IDomainEventDispatcher domainEventDispatcher,
        IStateTransitionValidator<TransactionStatus> stateTransitionValidator,
        IUpdateBalanceService updateBalanceService)
    {
        _transactionRepository = transactionRepository;
        _transactionApprovalPolicy = transactionApprovalPolicy;
        _domainEventDispatcher = domainEventDispatcher;
        _stateTransitionValidator = stateTransitionValidator;
        _updateBalanceService = updateBalanceService;
    }

    public Task<ApprovalDecision> ApproveWithRelatedTransactionsAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        // PROMENA STATUSA TREBA DA SE DESAVA NA SVIM TRANSAKCIJAMA I ONIM SUB I ORIGINALNOJ
        // trigger for every subtransactions... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // ne mora svaka subtransakcija da se apruvuje, ali one sto nisu u krajnjim statusima bi trebalo da se pomere sa trenutnog na approve

        throw new NotImplementedException();
    }

    public async Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var approvalDecision = await _transactionApprovalPolicy.EvaluateAsync(transaction, currentUserId, cancellationToken);

        if (approvalDecision.IsApproved)
        {
            if (!_stateTransitionValidator.IsValidTransition(transaction.Status, TransactionStatus.Approved))
                return ApprovalDecision.Reject($"Transaction cannot change from {transaction.Status} to Approved.");
            
            transaction.Status = TransactionStatus.Approved;
        }
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
        }
        await _transactionRepository.UpdateAsync(transaction);

        if (approvalDecision.IsApproved)
        {
            await _updateBalanceService.UpdateBalanceAndRelatedTransactionsAsync(transaction);

            await _domainEventDispatcher.RaiseAsync(new TransactionExecutedEvent(
                transaction.Id,
                transaction.AccountId,
                transaction.CounterpartyAccountDetails?.AccountNumber,
                transaction.InvolvedPartyId,
                transaction.CalculatedCurrencyAmount.Amount,
                transaction.CalculatedCurrencyAmount.Currency,
                true,
                DateTime.UtcNow
            )); // trigger only for main transaction; should it be triggered for related transactions as well ? ... notifications engine will collect trigger
        }

        return approvalDecision;
    }
}
