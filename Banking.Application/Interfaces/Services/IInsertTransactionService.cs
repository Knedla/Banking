using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;

namespace Banking.Application.Interfaces.Services;

public interface IInsertTransactionService<T> where T : IPolicy, ITransactionPolicy
{
    Task<U> AddAsync<U>(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default) where U : TransactionResponse, new();
}
