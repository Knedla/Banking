using Banking.Domain.Entities.Transactions;

namespace Banking.Application.Interfaces.Services;

public interface IFraudDetectionService
{
    Task<bool> IsSuspiciousTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
