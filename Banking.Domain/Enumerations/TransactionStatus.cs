namespace Banking.Domain.Enumerations;

public enum TransactionStatus
{
    Draft = 0,        // Created but not submitted
    Pending = 1,      // Awaiting approval/processing
    Approved = 2,     // Approved but not yet posted
    Processing = 3,   // In execution phase (e.g., async)
    Posted = 4,       // Funds moved, transaction posted
    Completed = 5,    // Fully settled/confirmed
    Reversed = 6,     // Undone after posting
    Cancelled = 7,    // Cancelled by user/system before posting
    Voided = 8,       // Invalidated (e.g., fraud, duplicate)
    Failed = 9,       // Technical failure
    Suspended = 10,   // On hold/fraud/compliance
    Scheduled = 11,   // Waiting for future execution
    Rejected = 12     // Blocked during validation/approval
}
