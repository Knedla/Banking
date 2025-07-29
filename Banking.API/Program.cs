using Banking.API.Controllers;
using Banking.API.Extension;
using Banking.Application.Commands.Common;
using Banking.Application.Common;
using Banking.Application.EventHandlers;
using Banking.Application.Events;
using Banking.Application.Extensions;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Application.Rules;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;
using Banking.Domain.Validators.StateMachine;
using Banking.Infrastructure.Events;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Infrastructure.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRepository();
builder.Services.AddServices();
builder.Services.AddTransactionCommands(typeof(ICommandHandler<,>).Assembly);
builder.Services.AddNotification(builder.Configuration);

// Policies
builder.Services.AddTransactionFeePolicy(builder.Configuration);
builder.Services.AddTransactionApprovalPolicy(builder.Configuration);

// StateMachine
builder.Services.AddSingleton<IStateTransitionValidator<TransactionStatus>, TransactionStatusTransitionValidator>();
builder.Services.AddSingleton<IStateTransitionValidator<ApprovalStatus>, ApprovalStatusTransitionValidator>();
builder.Services.AddSingleton<IStateTransitionValidator<WorkItemStatus>, WorkItemStatusTransitionValidator>();

// Domain
builder.Services.AddSingleton<IBankingDataStore, InMemoryBankingDataStore>();
builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

builder.Services.AddSingleton<IAccountNumberGenerator, AccountNumberGenerator>();
builder.Services.AddSingleton<ICurrencyExchangeService, CurrencyExchangeService>(); // TODO: add replacement for a certain period of time / replacement for a specified time

// Register the dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
// Register event handlers 
builder.Services.AddScoped<IDomainEventHandler<TransactionExecutedEvent>, TransactionExecutedHandler>();
builder.Services.AddScoped<IDomainEventHandler<CreateAccountRequestAddedEvent>, CreateAccountRequestAddedHandler>();

//builder.Services.AddScoped(typeof(IDomainEventHandler<>), typeof(WorkItemAddedHandler<>));

builder.Services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();





builder.Services.AddSingleton<IAccountBalanceFactory, AccountBalanceFactory>();

var ruleDefinitions = builder.Configuration.GetSection("RuleDefinitions").Get<RuleDefinitions>();
builder.Services.AddSingleton(ruleDefinitions);

builder.Services.AddSingleton<IRuleProcessor, RuleProcessor>();


// Build
var app = builder.Build();
//app.Run(); //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//






// Test
var transactionCommandHandlerFactory = app.Services.GetService(typeof(ITransactionCommandHandlerFactory)) as ITransactionCommandHandlerFactory;



// -- CreateAccount --
var createAccountCommandHandler = transactionCommandHandlerFactory.Create<CreateAccountRequestRequest, CreateAccountRequestResponse>();
var createAccountRequest = new CreateAccountRequestRequest()
{
    UserId = Guid.NewGuid(),
    InvolvedPartyId = new Guid("f4bd0b90-10c6-47ff-bb35-113c0c779c3f"),
    AccountType = AccountType.Standard,
    CurrencyCode = "USD",
    InitialDeposit = 500
};

CreateAccountController createAccountController = new CreateAccountController();
createAccountController.CreateAccount(createAccountCommandHandler, createAccountRequest, CancellationToken.None);


// -- AccountBalance --
var accountBalanceCommandHandler = transactionCommandHandlerFactory.Create<AccountBalanceRequest, AccountBalanceResponse>();

var accountBalanceRequest = new AccountBalanceRequest()
{
    AccountId = Guid.NewGuid(),
    RequestingUserId = Guid.NewGuid()
};

AccountBalanceController accountBalanceController = new AccountBalanceController();
accountBalanceController.GetBalance(accountBalanceCommandHandler, accountBalanceRequest, CancellationToken.None);



/*
 
TransactionStatusTransitionValidator
ApprovalStatusTransitionValidator
ApprovalService call


get exchange rate service


jos jedan sloj izmedju entiteta i objekata u servisima

*/