using Banking.Domain.Enumerations;

namespace Banking.Domain.ValueObjects;

public class Fee
{
    public string Name { get; set; }
    public string Code { get; set; } // e.g., "TRANSFER_FEE"
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    public Guid AccountId { get; set; }
    public FeeType Type { get; set; }
    public FeeTrigger Trigger { get; set; }
}
