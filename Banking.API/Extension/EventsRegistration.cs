using Banking.Application.Common;
using Banking.Application.EventHandlers;
using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions;

public static class EventsRegistration
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        // Register the dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        // Register event handlers 
        services.AddScoped<IDomainEventHandler<TransactionApprovedEvent>, TransactionApprovedHandler>();
        services.AddScoped<IDomainEventHandler<TransactionApprovedEvent>, TransactionWithRelatedTransactionsApprovedHandler>();
        services.AddScoped<IDomainEventHandler<TransactionExecutedEvent>, TransactionExecutedHandler>();
        services.AddScoped<IDomainEventHandler<CreateAccountRequestAddedEvent>, CreateAccountRequestAddedHandler>();
        services.AddScoped<IDomainEventHandler <TransactionApprovalRequestEvent>, TransactionApprovalRequestHandler>();
        services.AddScoped<IDomainEventHandler<TransactionFeeRequestEvent>, TransactionFeeRequestHandler>();
        
        //services.AddScoped(typeof(IDomainEventHandler<>), typeof(WorkItemAddedHandler<>)); // TODO: check why is not working ...

        services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();

        return services;
    }
}
