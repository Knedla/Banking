using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Domain.Entities.WorkItems;

namespace Banking.Application.EventHandlers;

public class CreateAccountRequestAddedHandler : WorkItemAddedHandler<CreateAccountRequestAddedEvent, CreateAccountRequest>
{
    public CreateAccountRequestAddedHandler(IRuleProcessor ruleProcessor) : base(ruleProcessor) { }
}
