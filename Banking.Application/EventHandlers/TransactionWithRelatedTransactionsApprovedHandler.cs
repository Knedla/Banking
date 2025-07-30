using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;

namespace Banking.Application.EventHandlers;

public class TransactionWithRelatedTransactionsApprovedHandler : IDomainEventHandler<TransactionApprovedEvent>
{
    private readonly ITransactionService _transactionService;

    public TransactionWithRelatedTransactionsApprovedHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task HandleAsync(TransactionApprovedEvent domainEvent)
    {
        await _transactionService.CompleteTransactionWithRelatedTransactions(domainEvent.TransactionId);
    }
}
