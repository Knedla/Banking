using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionApprovedEvent(
    Guid TransactionId,
    Guid InvolvedPartyId
) : IDomainEvent;