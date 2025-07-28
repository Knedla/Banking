namespace Banking.Domain.Entities.Transactions;

public class TransactionBatch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // e.g., "Payroll July 2025"
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Common navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
