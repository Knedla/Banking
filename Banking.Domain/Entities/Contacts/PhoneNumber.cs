using Banking.Domain.Enumerations;

namespace Banking.Domain.Entities.Contacts;

public class PhoneNumber
{
    public Guid Id { get; set; }
    public Guid? InvolvedPartyId { get; set; }
    public string CountryCode { get; set; }
    public string Number { get; set; }
    public PhoneType Type { get; set; }
    public bool IsVerified { get; set; }
    public ContactPurpose Purposes { get; set; }
}
