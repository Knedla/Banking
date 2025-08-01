﻿using Banking.Domain.Entities.Transactions;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.WorkItems;

public class ReversalRequest : WorkItem // not used
{
    [Required]
    public Guid TransactionId { get; set; }

    [Required]
    public Guid RequestedByUserId { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public string? ReviewerComment { get; set; }


    // References
    public Transaction Transaction { get; set; }
}
