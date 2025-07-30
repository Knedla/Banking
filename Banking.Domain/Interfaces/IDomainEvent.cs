namespace Banking.Domain.Interfaces;

public interface IDomainEvent
{
    Guid InvolvedPartyId { get; } // needs to be moved out of here; used as a temp helper prop
}
