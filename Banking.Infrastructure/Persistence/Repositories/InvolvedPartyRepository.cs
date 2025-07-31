using Banking.Domain.Entities.Parties;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class InvolvedPartyRepository : GenericRepository<InvolvedParty>, IInvolvedPartyRepository
{
    public InvolvedPartyRepository(IBankingDataStore dataStore) : base(dataStore) { }

}
