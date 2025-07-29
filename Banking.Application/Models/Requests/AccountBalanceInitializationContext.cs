using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Requests;

public class AccountBalanceInitializationContext
{
    public Guid AccountId { get; set; }
    public AccountType AccountType { get; set; }
}
