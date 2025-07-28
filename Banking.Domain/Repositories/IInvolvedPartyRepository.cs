using Banking.Domain.Entities.Parties;

namespace Banking.Domain.Repositories;

public interface IInvolvedPartyRepository
{
    Task<InvolvedParty?> GetByIdAsync(Guid involvedPartyId);
    Task<IEnumerable<InvolvedParty>> GetAllAsync();
    Task AddAsync(InvolvedParty involvedParty);
    Task UpdateAsync(InvolvedParty involvedParty);
    Task DeleteAsync(Guid involvedPartyId);
    Task<bool> ExistsAsync(Guid involvedPartyId);
}
