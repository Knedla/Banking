namespace Banking.Application.Models.Requests
{
    public record DepositRequest(string AccountId, decimal Amount);
}
