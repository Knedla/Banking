namespace Banking.Domain.Models;

public class ApprovalRequirement
{
    public string RuleName { get; set; }
    public List<string> RequiredRoles { get; set; }
    public List<string> ApprovalGroups { get; set; }
    public int MinimumApprovals { get; set; } = 1;
    public string? Justification { get; set; }
}
