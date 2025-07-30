using Banking.Application.Models.Common;
using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class WithdrawalRequest : BaseRequest // Question: is it possible to withdraw from EUR balance and ask to give you USD ?
{
    public Guid? TransactionInitializedById { get; set; }
    public TransactionChannel TransactionChannel { get; set; }
    public TransactionAccountDetails TransactionAccountDetails { get; set; }

    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
}
