namespace Banking.Application.Models.Responses;

public class WithdrawalResponse : BaseResponse
{
    public Guid AccountId { get; set; }
    public decimal NewBalance { get; set; }
    public string ConfirmationNumber { get; set; }
}
