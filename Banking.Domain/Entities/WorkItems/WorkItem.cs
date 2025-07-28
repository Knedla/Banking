using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.WorkItems;

public class WorkItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid CreatedByUserId { get; set; }

    [Required]
    public DateTime ModifiedAt { get; set; }

    [Required]
    public Guid ModifiedByUserId { get; set; }

    // public ICollection<ApprovalPolicy> Policies { get; set; }
    // public ICollection<RequestValidator> Validators { get; set; }
}
