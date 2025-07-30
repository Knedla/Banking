using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record LowBalanceEvent(Guid InvolvedPartyId, decimal CurrentBalance, decimal Threshold) : IDomainEvent; // not used
