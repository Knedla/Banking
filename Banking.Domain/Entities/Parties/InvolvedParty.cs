using Banking.Domain.Entities.Contacts;
using Banking.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Parties;

public abstract class InvolvedParty
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; }

    [Required]
    public InvolvedPartyType Type { get; set; }

    // Common navigation properties
    public ICollection<IdentificationDocument> IdentificationDocuments { get; set; } = new List<IdentificationDocument>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    public ICollection<EmailAddress> Emails { get; set; } = new List<EmailAddress>();
}
