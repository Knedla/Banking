namespace Banking.Application.Models.Requests
{
    public record CreateAccountRequest(string CustomerId, string AccountType);
}
