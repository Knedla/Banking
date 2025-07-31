using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Repositories;

namespace Banking.Application.Services;

// TODO: create factory pattern that request TransactionType and returns instance of IInsertTransactionService -> current IInsertTransactionService needs refactor to suport this
public class InsertFeeTransactionService : IInsertFeeTransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUpdateBalanceService _updateBalanceService;

    public InsertFeeTransactionService(
        ITransactionRepository transactionRepository,
        IUpdateBalanceService updateBalanceService)
    {
        _transactionRepository = transactionRepository;
        _updateBalanceService = updateBalanceService;
    }

    public async Task<T> AddAsync<T>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where T : TransactionResponse, new()
    {
        if (transaction == null)
            throw new Exception($"Transaction is null.");

        if (transaction.Type != TransactionType.Fee)
            throw new Exception($"Support only Fee transactions.");

        await _transactionRepository.AddAsync(transaction); // trigger event transaction added if needed
        await _updateBalanceService.UpdateBalanceAsync(transaction);

        return new T { TransactionStatus = transaction.Status, };
    }
}
