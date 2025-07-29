using Banking.Domain.Events;

namespace Banking.Application.Events;

public record DailyLimitReachedEvent(Guid InvolvedPartyId, decimal Limit, decimal Used) : IDomainEvent;
