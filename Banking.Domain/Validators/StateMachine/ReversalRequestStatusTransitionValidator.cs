using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Validators.StateMachine;

public class ReversalRequestStatusTransitionValidator : IStateTransitionValidator<ReversalRequestStatus>
{
    private static readonly Dictionary<ReversalRequestStatus, ReversalRequestStatus[]> _validTransitions = new()
    {
        [ReversalRequestStatus.Pending] = [ReversalRequestStatus.Approved, ReversalRequestStatus.Rejected, ReversalRequestStatus.Cancelled],
        [ReversalRequestStatus.Approved] = [ReversalRequestStatus.Executed],
        [ReversalRequestStatus.Rejected] = Array.Empty<ReversalRequestStatus>(),
        [ReversalRequestStatus.Cancelled] = Array.Empty<ReversalRequestStatus>(),
        [ReversalRequestStatus.Executed] = Array.Empty<ReversalRequestStatus>()
    };

    public bool IsValidTransition(ReversalRequestStatus current, ReversalRequestStatus next)
    {
        return _validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
