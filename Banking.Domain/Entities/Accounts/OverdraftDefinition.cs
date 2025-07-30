using Banking.Domain.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Accounts;

public class OverdraftDefinition // not used
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public AccountType AccountType { get; set; }

    // [Required]
    // public OverdraftType Type { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal MaxOverdraftAmount { get; set; }

    [Required, StringLength(3)]
    public string CurrencyCode { get; set; }
}
