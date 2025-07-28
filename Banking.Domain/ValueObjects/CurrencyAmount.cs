using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.ValueObjects;

public class CurrencyAmount
{
    [Required]
    [StringLength(3)]
    public string CurrencyCode { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    // resolved references
    public Currency Currency { get; set; }
}
