namespace Banking.Application.Models.Requests;

public class WithdrawalRequest : BaseRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
}
