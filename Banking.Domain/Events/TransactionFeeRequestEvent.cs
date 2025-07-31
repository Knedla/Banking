using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionFeeRequestEvent(
    Guid TransactionId,
    Guid CurrentUserId,
    Guid InvolvedPartyId
) : IDomainEvent;