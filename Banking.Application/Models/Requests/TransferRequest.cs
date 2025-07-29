namespace Banking.Application.Models.Requests;

public class TransferRequest : BaseRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}
