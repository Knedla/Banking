namespace Banking.Domain.Models;

public class ApprovalDecision
{
    public bool IsApproved { get; init; }
    public string? Reason { get; init; }
    public static ApprovalDecision Approve() => new() { IsApproved = true };
    public static ApprovalDecision Reject(string reason) => new() { IsApproved = false, Reason = reason };
}
