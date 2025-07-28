namespace Banking.Application.Models.Requests;

public class CreateAccountRequest
{
    public Guid CustomerId { get; set; }
    public string OwnerName { get; set; }
    public string AccountType { get; set; }
    public decimal InitialDeposit { get; set; }
}
