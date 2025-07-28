using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.ValueObjects;

public class ExchangeRate
{
    [Required, StringLength(3)]
    public string FromCurrencyCode { get; set; }

    [Required, StringLength(3)]
    public string ToCurrencyCode { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Rate { get; set; }

    // resolved references
    public Currency FromCurrency { get; set; }
    public Currency ToCurrency { get; set; }
}
