using Banking.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Transactions;

public class TransactionApproval
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid TransactionId { get; set; }

    [Required]
    public Guid RequirementId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid ApproverId { get; set; }

    [Required]
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

    public DateTime? ActionedAt { get; set; }

    [StringLength(100)]
    public string? Comments { get; set; }

    // resolved references
    public Transaction Transaction { get; set; }
    public TransactionApprovalRequirement Requirement { get; set; }
}
