using Banking.Domain.Enumerations;

namespace Banking.Domain.Entities;

public class Fee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; } // e.g., "TRANSFER_FEE"
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    public FeeType Type { get; set; }
    public FeeTrigger Trigger { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
