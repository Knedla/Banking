using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.WorkItems;

public class ReversalRequest : WorkItem
{
    [Required]
    public Guid TransactionId { get; set; }

    [Required]
    public Guid RequestedByUserId { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    public ReversalRequestStatus Status { get; set; } = ReversalRequestStatus.Pending;

    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public string? ReviewerComment { get; set; }


    // resolved references
    public Transaction Transaction { get; set; }
}
