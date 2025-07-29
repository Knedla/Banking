using Banking.Domain.Entities.Accounts;
using Banking.Domain.Enumerations;
using Banking.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Transactions;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }

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

    [Required]
    public Guid AccountId { get; set; }
    
    public string? Description { get; set; }

    public RecipientDetails? RecipientDetails { get; set; } // Receiver account information -> save as json

    [Required]
    public CurrencyAmount InitCurrencyAmount { get; set; }

    public ExchangeRate? ExchangeRate { get; set; }         // Conversion rate to base currency (e.g., RSD).

    [Required]
    public CurrencyAmount CalculatedCurrencyAmount { get; set; } // Calculated base currency amount CurrencyAmount.Amount * ExchangeRate?.Rate ?? 1

    [Required] // TODO: domain rules, enforce that: RequiresApproval can only be true when ApprovalStatus != ApprovalStatus.NotRequired
    public bool RequiresApproval { get; set; }              // Calculated

    public ApprovalStatus ApprovalStatus { get; set; }

    [Required] // TODO: domain rules, enforce that: IsDeleted can only be true when TransactionStatus == TransactionStatus.Cancelled; depending on business, maybe some other statuses can be added as well
    public bool IsDeleted { get; set; }                     // Calculated
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid CreatedByUserId { get; set; }

    [Required]
    public DateTime LastModifiedAt { get; set; }

    [Required]
    public Guid LastModifiedByUserId { get; set; }

    // Common navigation properties
    public ICollection<Transaction> RelatedTransactions { get; set; }
    public ICollection<TransactionApprovalRequirement> ApprovalRequirements { get; set; }
    public ICollection<TransactionApproval> Approvals { get; set; }
    public ICollection<TransactionBatch> Batches { get; set; }

    // resolved references
    public Transaction? RelatedToTransaction { get; set; }
    public Transaction? ReversalTransaction { get; set; }
    public Account Account { get; set; }
}
