using Banking.Domain.Entities.WorkItems;

namespace Banking.Application.Models.Requests;

public class CreateAccountRequest : BaseRequest
{
    public Guid WorkItemId { get; set; }
    public WorkItem WorkItem { get; set; } // TODO: make DTO
}
