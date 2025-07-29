using Banking.Domain.Entities.Parties;

namespace Banking.Application.Models.Responses;

public class InvolvedPartyResponse : BaseResponse
{
    public InvolvedParty InvolvedParty { get; set; }
}
