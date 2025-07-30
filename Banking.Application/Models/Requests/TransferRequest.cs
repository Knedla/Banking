using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class TransferRequest : BaseRequest
{
    public Guid AccountId { get; set; } // if not passed, it needs to be able to be resolved from AccountNumber or some other alternative key
    public Guid InvolvedPartyId { get; set; }
    public TransactionChannel TransactionChannel { get; set; }

    // alternative key
    public string AccountNumber { get; set; }
    public string IBAN { get; set; }

    public decimal Amount { get; set; }
    public string FromCurrencyCode { get; set; }
    public string? ToCurrencyCode { get; set; }

    public CounterpartyAccountDetails CounterpartyAccountDetails { get; set; }
}
