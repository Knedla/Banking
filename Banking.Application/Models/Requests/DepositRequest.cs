using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class DepositRequest : BaseRequest
{
    public Guid AccountId { get; set; }
    public Guid InvolvedPartyId { get; set; }
    public TransactionChannel TransactionChannel { get; set; }

    // alternative key
    public string AccountNumber { get; set; }
    public string IBAN { get; set; }

    public decimal Amount { get; set; }
    public string FromCurrencyCode { get; set; }
    public string? ToCurrencyCode { get; set; }
}
