using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Transactions;

public class TransactionApprovalRequirement
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TransactionId { get; set; }

    [Required]
    public string RuleName { get; set; } = default!;

    [Required]
    public List<string> RequiredRoles { get; set; } // Configure EF to store the list as a delimited string in a single column. => Suitable if you don’t need to query individual roles at the DB level

    [Required]
    public List<string> ApprovalGroups { get; set; } // Configure EF to store the list as a delimited string in a single column. => Suitable if you don’t need to query individual roles at the DB level

    [Required]
    public int MinimumApprovals { get; set; } = 1;

    [Required]
    public string? Justification { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
