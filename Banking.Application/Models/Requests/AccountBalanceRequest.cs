namespace Banking.Application.Models.Requests;

public class AccountBalanceRequest : BaseRequest
{
    public Guid AccountId { get; set; }
    public Guid InvolvedPartyId { get; set; }
}
