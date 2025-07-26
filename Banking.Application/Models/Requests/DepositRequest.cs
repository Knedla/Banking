namespace Banking.Application.Models.Requests
{
    public class DepositRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
