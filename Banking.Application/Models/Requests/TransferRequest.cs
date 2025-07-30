using Banking.Application.Models.Common;
using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class TransferRequest : BaseRequest
{
    public Guid? TransactionInitializedById { get; set; }
    public TransactionChannel TransactionChannel { get; set; }
    public TransactionAccountDetails FromTransactionAccountDetails { get; set; }
    public TransactionAccountDetails ToTransactionAccountDetails { get; set; }
    public CounterpartyAccountDetails CounterpartyAccountDetails { get; set; }
    public decimal Amount { get; set; }
    public string FromCurrencyCode { get; set; }
    public string? ToCurrencyCode { get; set; }
}
