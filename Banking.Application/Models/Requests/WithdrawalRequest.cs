namespace Banking.Application.Models.Requests
{
    public record WithdrawalRequest(string AccountId, decimal Amount);
}
