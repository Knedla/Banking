using Banking.Domain.Entities.Transactions;
namespace Banking.Application.Interfaces.Services;

public interface ITransactionFeeService
{
    Task AddFeesAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default);
}
