using Banking.Domain.Entities.Parties;
using Banking.Domain.Enumerations;
using Banking.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Accounts;

public class Account : BaseEntity
{
    [Required]
    public Guid InvolvedPartyId { get; set; }

    [StringLength(50)]
    public string? AccountNumber { get; set; }

    [StringLength(34, MinimumLength = 15)]
    public string? IBAN { get; set; }

    [StringLength(11, MinimumLength = 8)]
    public string? SWIFTCode { get; set; }

    [Required]
    public AccountType AccountType { get; set; }

    public AccountTier? AccountTier { get; set; }

    [Required]
    public string PrimaryCurrencyCode { get; set; }

    public Overdraft? Overdraft { get; set; }

    [Required]
    public bool IsFrozen { get; set; } // TODO: include in logic

    // Common navigation properties
    [Required] 
    public ICollection<AccountBalance> Balances { get; set; } // constraints: there must be an AccountBalance whose CurrencyCode == PrimaryCurrencyCode
    public ICollection<TransactionHolding> Holdings { get; set; }

    //public ICollection<AccountMandate> Mandates { get; set; }
    //public ICollection<AccountPolicy> Policies { get; set; }

    // References
    public InvolvedParty InvolvedParty { get; set; }
    public Currency? PrimaryCurrency { get; set; }
}
