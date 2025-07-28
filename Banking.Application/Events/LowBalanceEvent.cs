using Banking.Domain.Events;

namespace Banking.Application.Events;

public record LowBalanceEvent(Guid AccountId, decimal CurrentBalance, decimal Threshold) : IDomainEvent;
