namespace Banking.Application.Models.Responses
{
    public record TransferResponse(string FromAccountId, string ToAccountId, decimal FromBalance, decimal ToBalance);
}
