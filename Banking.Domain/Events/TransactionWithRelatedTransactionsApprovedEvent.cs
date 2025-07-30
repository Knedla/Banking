using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionWithRelatedTransactionsApprovedEvent(
    Guid TransactionId,
    Guid InvolvedPartyId
) : IDomainEvent;