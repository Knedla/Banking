using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities
{
    public class AccountType // alternatively it could be an enumeration
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public AccountType Clone()
        {
            return new AccountType()
            {
                Id = Id,
                Name = Name,
                IsActive = IsActive
            };
        }
    }
}
