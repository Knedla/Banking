using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record DailyLimitReachedEvent(Guid InvolvedPartyId, decimal Limit, decimal Used) : IDomainEvent;
