using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities
{
    public class Customer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public List<Account> Accounts { get; set; } = new();
    }
}
