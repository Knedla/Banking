using Banking.API.Controllers;
using Banking.API.Extension;
using Banking.Application.Commands.Common;
using Banking.Application.Extensions;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Application.Notifications.Channels;
using Banking.Application.Notifications.Interfaces;
using Banking.Application.Notifications.Rules;
using Banking.Application.Notifications;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;
using Banking.Domain.Validators.StateMachine;
using Banking.Infrastructure.Decorators;
using Banking.Infrastructure.Factories;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Banking.Application.Commands.Transfer;
using Banking.Application.Events;
using Banking.Domain.Events;
using Banking.Application.Common;
using Banking.Infrastructure.Events;
using Banking.Application.Interfaces;
using Banking.Application.Common.Expressions;
using Banking.Application.Notifications.Routing;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRepository();
builder.Services.AddServices();
builder.Services.AddTransactionCommands(typeof(ICommandHandler<,>).Assembly);

// Policies
builder.Services.AddTransactionFeePolicy(builder.Configuration);
builder.Services.AddTransactionApprovalPolicy(builder.Configuration);




// Factory
builder.Services.AddScoped<ITransactionCommandHandlerFactory, TransactionCommandHandlerFactory>();

// StateMachine
builder.Services.AddSingleton<IStateTransitionValidator<TransactionStatus>, TransactionStatusTransitionValidator>();
builder.Services.AddSingleton<IStateTransitionValidator<ApprovalStatus>, ApprovalStatusTransitionValidator>();
builder.Services.AddSingleton<IStateTransitionValidator<ReversalRequestStatus>, ReversalRequestStatusTransitionValidator>();

// Domain
builder.Services.AddSingleton<IBankingDataStore, InMemoryBankingDataStore>();
builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();









builder.Services.AddScoped<INotificationDestinationResolver, NotificationDestinationResolver>();
builder.Services.AddScoped<INotificationRuleProvider, NotificationRuleProvider>();
builder.Services.AddScoped<ITemplateRenderer, SimpleTemplateRenderer>();
builder.Services.AddScoped<INotificationChannelDispatcher, NotificationChannelDispatcher>();


builder.Services.Configure<List<NotificationRule>>(builder.Configuration.GetSection("NotificationRules"));
builder.Services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();
builder.Services.AddScoped(typeof(INotificationEngine<>), typeof(NotificationEngine<>));

// Channel mocks or real
builder.Services.AddSingleton<IEmailSender, MockEmailSender>();
builder.Services.AddSingleton<ISmsSender, MockSmsSender>();
builder.Services.AddSingleton<IInAppNotifier, MockInAppNotifier>();

// Register the dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Register event handlers (example)
builder.Services.AddScoped<IDomainEventHandler<TransactionExecutedEvent>, TransactionExecutedHandler>();
//builder.Services.AddScoped<IDomainEventHandler<LowBalanceEvent>, LowBalanceHandler>();
//builder.Services.AddScoped<IDomainEventHandler<DailyLimitReachedEvent>, DailyLimitHandler>();













builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IExecutionTransactionCommandHandler<,>))
    .AddClasses(classes => classes.AssignableTo(typeof(IExecutionTransactionCommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.Decorate(
    typeof(IExecutionTransactionCommandHandler<,>),
    typeof(TransactionalExecutionCommandHandlerDecorator<,>));

// Build
var app = builder.Build();
//app.Run(); //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//






// Test
var transactionCommandHandlerFactory = app.Services.GetService(typeof(ITransactionCommandHandlerFactory)) as ITransactionCommandHandlerFactory;
var commandHandler = transactionCommandHandlerFactory.Create<AccountBalanceRequest, AccountBalanceResponse>();

var request = new AccountBalanceRequest()
{
    AccountId = Guid.NewGuid(),
    RequestingUserId = Guid.NewGuid()
};

AccountBalanceController accountBalanceController = new AccountBalanceController();
accountBalanceController.GetBalance(commandHandler, request, CancellationToken.None);



/*
 
TransactionStatusTransitionValidator
ApprovalStatusTransitionValidator
ApprovalService call


get exchange rate service


jos jedan sloj izmedju entiteta i objekata u servisima

*/