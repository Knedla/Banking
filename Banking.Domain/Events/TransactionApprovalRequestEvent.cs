using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionApprovalRequestEvent(
    Guid TransactionId,
    Guid CurrentUserId,
    Guid InvolvedPartyId
) : IDomainEvent;