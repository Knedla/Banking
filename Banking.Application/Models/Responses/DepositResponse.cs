namespace Banking.Application.Models.Responses
{
    public class DepositResponse
    {
        public Guid AccountId { get; set; }
        public decimal NewBalance { get; set; }
    }
}
