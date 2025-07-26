namespace Banking.Application.Models.Responses
{
    public class TransferResponse
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal FromNewBalance { get; set; }
        public decimal ToNewBalance { get; set; }
    }
}
