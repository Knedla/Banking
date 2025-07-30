using Banking.Domain.Entities.Accounts;
using Banking.Domain.Enumerations;
using Banking.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Transactions;

public class Transaction : BaseEntity
{
    public Guid? TransactionInitializedById { get; set; } // Id of InvolvedParty

    // General linking (e.g. fee → transaction, interest → balance) // TODO: domain rules, enforce that: ReversalTransactionId can only be set when TransactionType != Reversal
    public Guid? RelatedToTransactionId { get; set; }
    
    // Reversal-specific linking (e.g. refund, chargeback) // TODO: domain rules, enforce that: ReversalTransactionId can only be set when TransactionType == Reversal
    public Guid? ReversalTransactionId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    [Required]
    public TransactionChannel Channel { get; set; }

    public TransactionAccountDetails? FromTransactionAccountDetails { get; set; }
    public TransactionAccountDetails? ToTransactionAccountDetails { get; set; }

    public string? Description { get; set; }

    public CounterpartyAccountDetails? CounterpartyAccountDetails { get; set; } // save as json ?

    [Required]
    public CurrencyAmount FromCurrencyAmount { get; set; }

    public ExchangeRate? ExchangeRate { get; set; }         // Conversion rate to base currency (e.g., RSD).

    [Required]
    public CurrencyAmount CalculatedCurrencyAmount { get; set; } // Calculated base currency amount CurrencyAmount.Amount * ExchangeRate?.Rate ?? 1

    [Required] // TODO: domain rules, enforce that: RequiresApproval can only be true when ApprovalStatus != ApprovalStatus.NotRequired
    public bool RequiresApproval { get; set; }              // Calculated

    public ApprovalStatus ApprovalStatus { get; set; }

    [Required] // TODO: domain rules, enforce that: IsDeleted can only be true when TransactionStatus == TransactionStatus.Cancelled; depending on business, maybe some other statuses can be added as well
    public bool IsDeleted { get; set; }                     // Calculated

    // Common navigation properties
    public ICollection<Transaction> RelatedTransactions { get; set; }
    public ICollection<TransactionApprovalRequirement> ApprovalRequirements { get; set; }
    public ICollection<TransactionApproval> Approvals { get; set; }
    public ICollection<TransactionBatch> Batches { get; set; } // not used

    // References
    public Transaction? RelatedToTransaction { get; set; }
    public Transaction? ReversalTransaction { get; set; }
    public Account Account { get; set; }
}
