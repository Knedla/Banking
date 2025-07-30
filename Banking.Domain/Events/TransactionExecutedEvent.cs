using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionExecutedEvent(
    Guid TransactionId,
    Guid SourceAccountId,
    string DestinationAccountNumber,
    Guid InvolvedPartyId,
    decimal Amount,
    string Currency,
    bool Success,
    DateTime Timestamp
) : IDomainEvent;