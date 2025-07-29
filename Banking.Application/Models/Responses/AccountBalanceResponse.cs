using Banking.Domain.Entities.Accounts;

namespace Banking.Application.Models.Responses;

public class AccountBalanceResponse : BaseResponse
{
    public Guid AccountId { get; set; }
    public List<AccountBalance> Balance { get; set; } // TODO: convert it to DTO
}
