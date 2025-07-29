using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Plicies;
using Banking.Domain.Policies;

namespace Banking.Infrastructure.Policies;

public class FraudDetectionDepositPolicy : IDepositPolicy // not used, I left them here as an idea...
{
    private readonly IFraudDetectionService _fraudService;

    public FraudDetectionDepositPolicy(IFraudDetectionService fraudService)
    {
        _fraudService = fraudService;
    }

    public async Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        bool isSuspicious = await _fraudService.IsSuspiciousTransactionAsync(transaction, cancellationToken);
        return isSuspicious
            ? TransactionPolicyResult.Failure("Suspicious transaction blocked.")
            : TransactionPolicyResult.Success();
    }
}
