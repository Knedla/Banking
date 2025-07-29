using Banking.Domain.Enumerations;

namespace Banking.Domain.Entities.Contacts;

public class EmailAddress
{
    public Guid Id { get; set; }
    public Guid? InvolvedPartyId { get; set; }
    public string Email { get; set; }
    public EmailType Type { get; set; }
    public bool IsVerified { get; set; }
    public ContactPurpose Purposes { get; set; }
}
