using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

// TODO: create factory pattern that request TransactionType and returns instance of IInsertTransactionService -> current IInsertTransactionService needs refactor to suport this
public class InsertTransactionService<T> : IInsertTransactionService<T> where T : IPolicy, ITransactionPolicy
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUpdateBalanceService _updateBalanceService;
    private readonly IFraudDetectionService _fraudDetectionService;
    private readonly IPolicyService<T> _policyService;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public InsertTransactionService(
        ITransactionService transactionService,
        ITransactionRepository transactionRepository,
        IUpdateBalanceService updateBalanceService,
        IFraudDetectionService fraudDetectionService,
        IPolicyService<T> policyService,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _transactionService = transactionService;
        _transactionRepository = transactionRepository;
        _updateBalanceService = updateBalanceService;
        _fraudDetectionService = fraudDetectionService;
        _policyService = policyService;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<U> AddAsync<U>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where U : TransactionResponse, new()
    {
        if (transaction == null)
            throw new Exception($"Transaction is null.");

        if (transaction.Type == TransactionType.Fee)
            throw new Exception($"Do not support only Fee transactions.");

        await _transactionRepository.AddAsync(transaction); // trigger event transaction added if needed
        await _updateBalanceService.UpdateBalanceAsync(transaction);

        var amlResult = await _fraudDetectionService.IsSuspiciousTransactionAsync(transaction, CancellationToken.None); // AML calculation

        if (amlResult)
        {
            await _transactionService.ChangeStatusAsync(transaction, TransactionStatus.Suspended, currentUserId, cancellationToken);

            var result = new U
            {
                TransactionStatus = transaction.Status
            };
            result.AddError("ALM watching you!");
            return result;
        }

        await _domainEventDispatcher.RaiseAsync(new TransactionFeeRequestEvent(
                transaction.Id,
                currentUserId,
                transaction.TransactionInitializedById ?? Guid.Empty // InvolvedPartyId prop should be removed from IDomainEvent, eventually
            ));

        var transactionPolicyResult = await _policyService.EvaluatePoliciesAsync(transaction, currentUserId, CancellationToken.None);
        if (transactionPolicyResult.Any(s => !s.IsSuccess))
        {
            await _transactionService.ChangeStatusAsync(transaction, TransactionStatus.Voided, currentUserId, cancellationToken);

            var result = new U
            {
                TransactionStatus = transaction.Status
            };
            foreach (var item in transactionPolicyResult.Where(s => s!.IsSuccess))
                result.AddError(item.ErrorMessage);
            return result;
        }

        await _domainEventDispatcher.RaiseAsync(new TransactionApprovalRequestEvent(
                transaction.Id,
                currentUserId,
                transaction.TransactionInitializedById ?? Guid.Empty // InvolvedPartyId prop should be removed from IDomainEvent, eventually
            ));

        return new U { TransactionStatus = transaction.Status, };
    }
}
