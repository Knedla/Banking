namespace Banking.Domain.Events;

public interface IDomainEvent
{
    Guid InvolvedPartyId { get; }
}
