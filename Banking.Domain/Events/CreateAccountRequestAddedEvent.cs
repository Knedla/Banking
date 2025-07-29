using Banking.Domain.Entities.WorkItems;

namespace Banking.Application.Events;

public class CreateAccountRequestAddedEvent : WorkItemAddedEvent<CreateAccountRequest>
{
    public CreateAccountRequestAddedEvent(CreateAccountRequest workItem) : base(workItem) { }
}
