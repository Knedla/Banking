namespace Banking.Application.Models.Responses;

public class AccountBalanceResponse
{
    public Guid AccountId { get; set; }
    public decimal Balance { get; set; }
}
