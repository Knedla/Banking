using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities;

public class TransactionHoldingApplication
{
    [Required]
    public bool IsAppliedToBalance { get; set; }

    [Required]
    public bool IsAppliedToAvailableBalance { get; set; }
}
