using Banking.Domain.Entities.Accounts;

namespace Banking.Domain.Entities.Transactions;

public class TransactionAccountDetails
{
    public Guid? AccountId { get; set; }
    public string? AccountNumber { get; set; }          // local format
    public string? Iban { get; set; }                   // international
    public string? SwiftCode { get; set; }              // e.g. BIC

    // References
    public Account Account { get; set; }
}
