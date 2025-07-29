using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Responses;

public class CreateAccountRequestResponse : BaseResponse
{
    public Guid WorkItemId { get; set; }
    public WorkItemStatus WorkItemStatus { get; set; }
}
