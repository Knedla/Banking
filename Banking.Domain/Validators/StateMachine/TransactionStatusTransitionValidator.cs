using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Validators.StateMachine;

public class TransactionStatusTransitionValidator : IStateTransitionValidator<TransactionStatus>
{
    private static readonly Dictionary<TransactionStatus, TransactionStatus[]> _validTransitions = new()
    {
        [TransactionStatus.Draft] = [TransactionStatus.Pending, TransactionStatus.Scheduled, TransactionStatus.Cancelled, TransactionStatus.Voided],
        [TransactionStatus.Pending] = [TransactionStatus.Approved, TransactionStatus.Cancelled, TransactionStatus.Rejected, TransactionStatus.Failed],
        [TransactionStatus.Approved] = [TransactionStatus.Processing, TransactionStatus.Suspended, TransactionStatus.Cancelled],
        [TransactionStatus.Processing] = [TransactionStatus.Posted, TransactionStatus.Failed, TransactionStatus.Suspended],
        [TransactionStatus.Posted] = [TransactionStatus.Completed, TransactionStatus.Reversed],
        [TransactionStatus.Completed] = Array.Empty<TransactionStatus>(),
        [TransactionStatus.Reversed] = Array.Empty<TransactionStatus>(),
        [TransactionStatus.Cancelled] = Array.Empty<TransactionStatus>(),
        [TransactionStatus.Voided] = Array.Empty<TransactionStatus>(),
        [TransactionStatus.Failed] = Array.Empty<TransactionStatus>(),
        [TransactionStatus.Suspended] = [TransactionStatus.Pending, TransactionStatus.Rejected],
        [TransactionStatus.Scheduled] = [TransactionStatus.Pending, TransactionStatus.Cancelled],
        [TransactionStatus.Rejected] = Array.Empty<TransactionStatus>()
    };

    public bool IsValidTransition(TransactionStatus current, TransactionStatus next)
    {
        return _validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
