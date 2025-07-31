using Banking.Domain.Entities.Transactions;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities;

public class TransactionHolding
{
    [Required]
    public Guid TransactionId { get; set; }

    public TransactionHoldingApplication IncommingApplication { get; set; }

    public TransactionHoldingApplication OutgoingApplication { get; set; }

    // [Required]
    // public bool Completed { get; set; }

    // References
    public Transaction Transaction { get; set; }
}
