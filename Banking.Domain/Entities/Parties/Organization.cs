using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Parties;

public class Organization : InvolvedParty
{
    [Required, StringLength(200)]
    public string LegalName { get; set; } = default!;

    [Required]
    public DateTime IncorporationDate { get; set; }

    public string? BusinessType { get; set; }
}
