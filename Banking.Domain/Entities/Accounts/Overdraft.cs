using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Accounts;

public class Overdraft // mock; this is here just to preserve value. surely there would be some overdraft object
{
    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Limit { get; set; }

    [StringLength(3)]
    public string CurrencyCode { get; set; }
}
