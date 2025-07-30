using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Validators.StateMachine;

public class ApprovalStatusTransitionValidator : IStateValidator<ApprovalStatus>
{
    private static readonly Dictionary<ApprovalStatus, ApprovalStatus[]> _validTransitions = new()
    {
        [ApprovalStatus.NotRequired] = Array.Empty<ApprovalStatus>(),
        [ApprovalStatus.Pending] = [ApprovalStatus.Approved, ApprovalStatus.Rejected, ApprovalStatus.Cancelled],
        [ApprovalStatus.Approved] = Array.Empty<ApprovalStatus>(),
        [ApprovalStatus.Rejected] = Array.Empty<ApprovalStatus>(),
        [ApprovalStatus.Cancelled] = Array.Empty<ApprovalStatus>()
    };

    public bool IsValidTransition(ApprovalStatus current, ApprovalStatus next)
    {
        return _validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
