using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;

namespace Banking.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<T> AddAsync<T>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where T : TransactionResponse, new();
    Task UpdateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
    Task ChangeStatusAsync(Transaction transaction, TransactionStatus newStatus, Guid currentUserId, CancellationToken cancellationToken = default);
    Task CompleteTransactionWithRelatedTransactions(Guid primaryTransactionId);
}
