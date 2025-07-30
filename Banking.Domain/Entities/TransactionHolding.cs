using Banking.Domain.Entities.Transactions;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities;

public class TransactionHolding
{
    [Required]
    public Guid TransactionId { get; set; }

    [Required]
    public bool IsAppliedToBalance { get; set; }

    [Required]
    public bool IsAppliedToAvailableBalance { get; set; }

    // References
    public Transaction Transaction { get; set; }
}
