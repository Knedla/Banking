using System.ComponentModel.DataAnnotations;

namespace Banking.Entities
{
    public class Account
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AccountTypeId { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = "USD"; // ISO 4217 format

        public DateTimeOffset LastWithdrawalDate { get; set; }
        public decimal WithdrawalTotalToday { get; set; }

        public int Version { get; set; }

        public AccountType AccountType { get; set; }
        public Customer Customer { get; set; }
    }
}
