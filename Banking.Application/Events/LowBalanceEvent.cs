using Banking.Domain.Events;

namespace Banking.Application.Events;

public record LowBalanceEvent(Guid InvolvedPartyId, decimal CurrentBalance, decimal Threshold) : IDomainEvent;
