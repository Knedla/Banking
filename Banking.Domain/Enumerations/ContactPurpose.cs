namespace Banking.Domain.Enumerations;

[Flags]
public enum ContactPurpose
{
    None = 0,
    Billing = 1,
    Shipping = 2,
    Legal = 4,
    Primary = 8, // TODO: domain rules, enforce that: only one primary can be active per contact type of the involved party
    Emergency = 16
}
