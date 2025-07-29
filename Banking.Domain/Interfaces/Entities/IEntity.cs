using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Interfaces.Entities
{
    public interface IEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}
