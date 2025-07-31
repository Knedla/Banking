using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;

namespace Banking.Application.Interfaces.Services;

public interface IInsertFeeTransactionService
{
    Task<T> AddAsync<T>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where T : TransactionResponse, new();
}
