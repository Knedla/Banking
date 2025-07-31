using Banking.Application.Models.Common;
using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class DepositRequest : BaseRequest
{
    public Guid? TransactionInitializedById { get; set; }
    public TransactionChannel TransactionChannel { get; set; }
    public TransactionAccountDetails TransactionAccountDetails { get; set; }
    
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    //public string? ToCurrencyCode { get; set; }
}
