using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class WithdrawalRequest : BaseRequest
{
    public Guid AccountId { get; set; }
    public Guid InvolvedPartyId { get; set; }
    public TransactionChannel TransactionChannel { get; set; }

    // alternative key
    public string AccountNumber { get; set; }
    public string IBAN { get; set; }

    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
}
