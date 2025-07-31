using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Repositories;

namespace Banking.Application.EventHandlers;

public class TransactionApprovalRequestHandler : IDomainEventHandler<TransactionApprovalRequestEvent>
{
    private readonly ITransactionApprovalService _transactionApprovalService;
    private readonly ITransactionRepository _transactionRepository;


    public TransactionApprovalRequestHandler(
        ITransactionApprovalService transactionApprovalService,
        ITransactionRepository transactionRepository)
    {
        _transactionApprovalService = transactionApprovalService;
        _transactionRepository = transactionRepository;
    }

    public async Task HandleAsync(TransactionApprovalRequestEvent domainEvent)
    {
        var transaction = await _transactionRepository.GetByIdAsync(domainEvent.TransactionId);

        if (transaction == null)
            throw new Exception($"Cannot resolve transaction.");

        await _transactionApprovalService.ApproveWithRelatedTransactionsAsync(transaction, domainEvent.CurrentUserId, CancellationToken.None);
    }
}
