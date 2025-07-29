namespace Banking.Domain.Interfaces;

public interface IDomainEvent
{
    Guid InvolvedPartyId { get; }
}
