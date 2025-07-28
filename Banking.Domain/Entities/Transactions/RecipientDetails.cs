namespace Banking.Domain.Entities.Transactions;

public class RecipientDetails
{
    public string? Name { get; set; }
    public string? AccountNumber { get; set; }          // local format
    public string? Iban { get; set; }                   // international
    public string? SwiftCode { get; set; }              // e.g. BIC
    public string? BankName { get; set; }
    public string? BankAddress { get; set; }
    public string? PaymentReference { get; set; }       // what payer writes (e.g., "Invoice 123")
    public string? ExternalReferenceCode { get; set; }  // ID from SWIFT/SEPA/etc.
}
