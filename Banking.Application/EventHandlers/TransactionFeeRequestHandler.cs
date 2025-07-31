using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Repositories;

namespace Banking.Application.EventHandlers;

public class TransactionFeeRequestHandler : IDomainEventHandler<TransactionFeeRequestEvent>
{
    private readonly ITransactionFeeService _transactionFeeService;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionFeeRequestHandler(
        ITransactionFeeService transactionFeeService,
        ITransactionRepository transactionRepository)
    {
        _transactionFeeService = transactionFeeService;
        _transactionRepository = transactionRepository;
    }

    public async Task HandleAsync(TransactionFeeRequestEvent domainEvent)
    {
        var transaction = await _transactionRepository.GetByIdAsync(domainEvent.TransactionId);

        if (transaction == null)
            throw new Exception($"Cannot resolve transaction.");

        await _transactionFeeService.AddFeesAsync(transaction, domainEvent.CurrentUserId, CancellationToken.None);
    }
}
