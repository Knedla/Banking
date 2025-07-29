namespace Banking.Application.Models.Responses;

public class DepositResponse : BaseResponse
{
    public Guid AccountId { get; set; }
    public decimal NewBalance { get; set; }
}
