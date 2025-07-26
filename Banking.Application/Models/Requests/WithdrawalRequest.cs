namespace Banking.Application.Models.Requests
{
    public class WithdrawalRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
