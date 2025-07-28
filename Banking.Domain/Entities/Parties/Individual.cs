using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Parties;

public class Individual : InvolvedParty
{
    [Required, StringLength(100)]
    public string FirstName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
}
