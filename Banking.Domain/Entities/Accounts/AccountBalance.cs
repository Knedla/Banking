using Banking.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Accounts;

public class AccountBalance
{
    [Key]
    public Guid AccountId { get; set; }

    [Key, StringLength(3)]
    public string CurrencyCode { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal AvailableBalance { get; set; }

    // resolved references
    public Account Account { get; set; }
    public Currency Currency { get; set; }
}
