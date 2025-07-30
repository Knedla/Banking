namespace Banking.Application.Models.Common;

public class TransactionAccountDetails
{
    public Guid? AccountId { get; set; }                // if not passed, it needs to be able to be resolved from AccountNumber or some other alternative key
    public string? AccountNumber { get; set; }          // local format
    public string? Iban { get; set; }                   // international
    public string? SwiftCode { get; set; }              // e.g. BIC
}
