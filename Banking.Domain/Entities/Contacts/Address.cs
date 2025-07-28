using Banking.Domain.Enumerations;

namespace Banking.Domain.Entities.Contacts;

public class Address
{
    public Guid Id { get; set; }
    public Guid? InvolvedPartyId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string StateOrProvince { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; } // ISO Alpha-2
    public AddressType Type { get; set; }
    public bool IsVerified { get; set; }
    public List<ContactPurpose> Purposes { get; set; }
}
