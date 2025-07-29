using Banking.Domain.Enumerations;

namespace Banking.Domain.Entities.WorkItems;

public class CreateAccountRequest : WorkItem
{
    public Guid InvolvedPartyId { get; set; }
    public AccountType AccountType { get; set; }
    public decimal InitialDeposit { get; set; }
    public string FromCurrencyCode { get; set; }
    public string ToCurrencyCode { get; set; }
    public TransactionChannel TransactionChannel { get; set; }
}
