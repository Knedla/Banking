namespace Banking.Domain.Enumerations;

public enum ReversalRequestStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3,
    Executed = 4 // The reversal has been processed
}
