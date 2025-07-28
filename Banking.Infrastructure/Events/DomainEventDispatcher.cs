using Banking.Application.Common;
using Banking.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Infrastructure.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task RaiseAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        // Resolve all registered handlers for TEvent
        var handlers = _serviceProvider
            .GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent);
        }
    }
}
