using Banking.Domain.Events;

namespace Banking.Application.Events;

public record DailyLimitReachedEvent(Guid AccountId, decimal Limit, decimal Used) : IDomainEvent;
