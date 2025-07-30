namespace Banking.Application.Models.Common;

public class CounterpartyAccountDetails
{
    public string? Name { get; set; }
    public string? BankName { get; set; }
    public string? BankAddress { get; set; }
    public string? PaymentReference { get; set; }       // what payer writes (e.g., "Invoice 123")
    public string? ExternalReferenceCode { get; set; }  // ID from SWIFT/SEPA/etc.
    public string? Priority {  get; set; }
}
