namespace Banking.Application.Models.Requests;

public class DepositRequest : BaseRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
}
