using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Validators.StateMachine;

public class WorkItemStatusTransitionValidator : IStateValidator<WorkItemStatus>
{
    private static readonly Dictionary<WorkItemStatus, WorkItemStatus[]> _validTransitions = new()
    {
        [WorkItemStatus.Pending] = [WorkItemStatus.Approved, WorkItemStatus.Rejected, WorkItemStatus.Cancelled],
        [WorkItemStatus.Approved] = [WorkItemStatus.Executed],
        [WorkItemStatus.Rejected] = Array.Empty<WorkItemStatus>(),
        [WorkItemStatus.Cancelled] = Array.Empty<WorkItemStatus>(),
        [WorkItemStatus.Executed] = Array.Empty<WorkItemStatus>()
    };

    public bool IsValidTransition(WorkItemStatus current, WorkItemStatus next)
    {
        return _validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
