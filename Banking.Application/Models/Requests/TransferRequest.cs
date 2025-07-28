namespace Banking.Application.Models.Requests;

public class TransferRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}
