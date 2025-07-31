using Banking.Domain.Enumerations;

namespace Banking.Domain.Configuration;

public class FeeValue
{
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    public FeeType Type { get; set; }
    public Guid? AccountId { get; set; }
}
