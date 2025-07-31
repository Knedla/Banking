using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.WorkItems;

public abstract class WorkItem : BaseEntity, IRuleEntity // not used ...
{
    [Required]
    public WorkItemStatus Status { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    // Common navigation properties
    public ICollection<AppliedRule> AppliedRules { get; set; }
}
