using Banking.Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; }

        [Required]
        public DateTime LastModifiedAt { get; set; }

        [Required]
        public Guid LastModifiedByUserId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true; // TODO: include this prop to logic
    }
}
