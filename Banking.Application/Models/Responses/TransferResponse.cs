namespace Banking.Application.Models.Responses;

public class TransferResponse : BaseResponse
{
    public Guid TransactionId {  get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public Guid InvolvedPartyId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime Timestamp { get; set; }
}
