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

    public string? PrimaryCurrencyCode { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public Guid CreatedByUserId { get; set; }

    [Required]
    public DateTime LastModifiedAt { get; set; }

    [Required]
    public Guid LastModifiedByUserId { get; set; }

    // Common navigation properties
    [Required]
    public ICollection<AccountBalance> Balances { get; set; }
    //public ICollection<AccountMandate> Mandates { get; set; }
    //public ICollection<AccountPolicy> Policies { get; set; }

    // resolved references
    public InvolvedParty InvolvedParty { get; set; }
    public Currency? PrimaryCurrency { get; set; }
}
