using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUpdateBalanceService _updateBalanceService;
    private readonly ITransactionStateValidator _transactionStateValidator;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IUpdateBalanceService updateBalanceService,
        ITransactionStateValidator transactionStateValidator,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _transactionRepository = transactionRepository;
        _updateBalanceService = updateBalanceService;
        _transactionStateValidator = transactionStateValidator;
        _domainEventDispatcher = domainEventDispatcher;
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
