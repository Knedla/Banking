using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUpdateBalanceService _updateBalanceService;
    private readonly ITransactionStateValidator _transactionStateValidator;
    private readonly IFraudDetectionService _fraudDetectionService;
    private readonly ITransactionApprovalService _transactionApprovalService;
    private readonly ITransactionFeeService _transactionFeeService;
    private readonly IPolicyService<IWithdrawalPolicy> _policyService;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IUpdateBalanceService updateBalanceService,
        ITransactionStateValidator transactionStateValidator,
        IFraudDetectionService fraudDetectionService,
        ITransactionApprovalService transactionApprovalService,
        ITransactionFeeService transactionFeeService,
        IPolicyService<IWithdrawalPolicy> policyService,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _transactionRepository = transactionRepository;
        _updateBalanceService = updateBalanceService;
        _transactionStateValidator = transactionStateValidator;
        _fraudDetectionService = fraudDetectionService;
        _transactionApprovalService = transactionApprovalService;
        _transactionFeeService = transactionFeeService;
        _policyService = policyService;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<T> AddAsync<T>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where T : TransactionResponse, new()
    {
        await _transactionRepository.AddAsync(transaction); // trigger event transaction added if needed
        await _updateBalanceService.UpdateBalanceAsync(transaction);

        if (await _fraudDetectionService.IsSuspiciousTransactionAsync(transaction, CancellationToken.None)) // AML calculation
        {
            await ChangeStatusAsync(transaction, TransactionStatus.Suspended, currentUserId, cancellationToken);

            var result = new T
            {
                TransactionStatus = transaction.Status
            };
            result.AddError("ALM watching you!");
            return result;
        }
        await _transactionFeeService.AddFeesAsync(transaction, currentUserId, CancellationToken.None);

        var transactionPolicyResult = await _policyService.EvaluatePoliciesAsync(transaction, currentUserId, CancellationToken.None);
        if (transactionPolicyResult.Any(s => !s.IsSuccess))
        {
            await ChangeStatusAsync(transaction, TransactionStatus.Voided, currentUserId, cancellationToken);

            var result = new T
            {
                TransactionStatus = transaction.Status
            };
            foreach (var item in transactionPolicyResult.Where(s => s!.IsSuccess))
                result.AddError(item.ErrorMessage);
            return result;
        }

        await _transactionApprovalService.ApproveWithRelatedTransactionsAsync(transaction, currentUserId, CancellationToken.None);

        return new T { TransactionStatus = transaction.Status, };
    }

    public async Task UpdateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task ChangeStatusAsync(Transaction transaction, TransactionStatus newStatus, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        if (!_transactionStateValidator.IsValidTransition(transaction.Status, newStatus))
            throw new Exception($"Transaction cannot change from {transaction.Status} to {newStatus}.");

        transaction.Status = newStatus;
        await _transactionRepository.UpdateAsync(transaction);
        await _updateBalanceService.UpdateBalanceAsync(transaction);
    }

    public async Task CompleteTransactionWithRelatedTransactions(Guid primaryTransactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(primaryTransactionId);

        if (transaction == null)
            throw new Exception($"Cannot resolve transaction.");

        var systemUserId = Guid.NewGuid(); // TODO: resolve systemUserId

        await ChangeStatusAsync(transaction, TransactionStatus.Completed, systemUserId); // overhead to execute UpdateAsync every time 

        foreach (var item in transaction.RelatedTransactions)
            await ChangeStatusAsync(item, TransactionStatus.Completed, systemUserId); // overhead to execute UpdateAsync every time 

        await _domainEventDispatcher.RaiseAsync(new TransactionExecutedEvent(
                transaction.Id,
                transaction.TransactionInitializedById ?? Guid.Empty // InvolvedPartyId prop should be removed from IDomainEvent, eventually
            ));
    }
}
