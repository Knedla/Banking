namespace Banking.Application.Models.Requests;

public class AccountBalanceRequest
{
    public Guid AccountId { get; set; }
    public Guid RequestingUserId { get; set; }
}
