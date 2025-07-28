namespace Banking.Application.Models.Responses;

public class CreateAccountResponse
{
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; }
    public decimal InitialBalance { get; set; }
}
