namespace Banking.Application.Models.Requests
{
    public record TransferRequest(string FromAccountId, string ToAccountId, decimal Amount);
}
