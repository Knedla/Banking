using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class CreateAccountRequestRequest : BaseRequest // TODO: change naming convention
{
    public Guid InvolvedPartyId { get; set; }
    public AccountType AccountType { get; set; }
    public string CurrencyCode { get; set; }
    public decimal InitialDeposit { get; set; }
}
