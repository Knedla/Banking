using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class InvolvedPartyService : IInvolvedPartyService
{
    private readonly IInvolvedPartyRepository _involvedPartyRepository;

    public InvolvedPartyService(IInvolvedPartyRepository involvedPartyRepository)
    {
        _involvedPartyRepository = involvedPartyRepository;
    }

    public async Task<InvolvedPartyResponse> GetInvolvedPartyAsync(InvolvedPartyRequest request)
    {
        var involvedParty = await _involvedPartyRepository.GetByIdAsync(request.InvolvedPartyId);
        return new InvolvedPartyResponse
        {
            InvolvedParty = involvedParty // TODO: map to another object, do not leave it as an entity object
        };
    }
}
