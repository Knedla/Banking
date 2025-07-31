using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Validators.StateMachine;

public class TransactionStatusValidator : ITransactionStateValidator
{
    private static readonly Dictionary<TransactionStatus, TransactionStatus[]> _validTransitions = new()
    {
        [TransactionStatus.Draft] = [TransactionStatus.Pending, TransactionStatus.Scheduled, TransactionStatus.Cancelled, TransactionStatus.Voided],
        [TransactionStatus.Pending] = [TransactionStatus.Approved, TransactionStatus.Cancelled, TransactionStatus.Rejected, TransactionStatus.Failed],
        [TransactionStatus.Approved] = [TransactionStatus.Processing, TransactionStatus.Suspended, TransactionStatus.Cancelled, TransactionStatus.Completed], // completed status added here for convenience
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

    private static readonly HashSet<TransactionStatus> endTransactionStatuses = new HashSet<TransactionStatus>
    {
        TransactionStatus.Completed,
        TransactionStatus.Reversed,
        TransactionStatus.Cancelled,
        TransactionStatus.Voided,
        TransactionStatus.Failed,
        TransactionStatus.Rejected,
    };

    private static readonly HashSet<TransactionStatus> doNotApplyToAccountBalanceStatuses = new HashSet<TransactionStatus>
    {
        TransactionStatus.Draft,
    };

    private static readonly HashSet<TransactionStatus> applyToActualBalanceStatuses = new HashSet<TransactionStatus>
    {
        TransactionStatus.Processing,
        TransactionStatus.Scheduled,

        TransactionStatus.Approved,
        TransactionStatus.Pending,
        TransactionStatus.Posted,
        TransactionStatus.Suspended,
    };

    private static readonly HashSet<TransactionStatus> rollbackFromActualBalanceStatuses = new HashSet<TransactionStatus>
    {
        TransactionStatus.Reversed,
        TransactionStatus.Cancelled,
        TransactionStatus.Voided,
        TransactionStatus.Failed,
        TransactionStatus.Rejected,
    };

    private static readonly HashSet<TransactionStatus> commitToBalanceStatuses = new HashSet<TransactionStatus>
    {
        TransactionStatus.Completed
    };

    public HashSet<TransactionStatus> GetApplyToActualBalanceStatuses()
        => applyToActualBalanceStatuses;

    public HashSet<TransactionStatus> GetCommitToBalanceStatuses()
        => commitToBalanceStatuses;

    public HashSet<TransactionStatus> GetDoNotApplyToAccountBalanceStatuses()
        => doNotApplyToAccountBalanceStatuses;

    public HashSet<TransactionStatus> GetRollbackFromActualBalanceStatuses()
        => rollbackFromActualBalanceStatuses;

    public HashSet<TransactionStatus> GetEndTransactionStatuses()
        => endTransactionStatuses;

    public bool IsValidTransition(TransactionStatus current, TransactionStatus next)
        => _validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
}
