using Banking.API.Controllers;
using Banking.API.Extension;
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
builder.Services.AddTransactionCommands();
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

// Account
builder.Services.AddSingleton<IAccountNumberGenerator, AccountNumberGenerator>();
builder.Services.AddSingleton<ICurrencyExchangeService, CurrencyExchangeService>(); // TODO: must have a period/time of validity, the exchange rate changes on a daily basis
builder.Services.AddSingleton<IAccountBalanceFactory, AccountBalanceFactory>();

// Register the dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
// Register event handlers 
builder.Services.AddScoped<IDomainEventHandler<TransactionExecutedEvent>, TransactionExecutedHandler>();
builder.Services.AddScoped<IDomainEventHandler<CreateAccountRequestAddedEvent>, CreateAccountRequestAddedHandler>();
//builder.Services.AddScoped(typeof(IDomainEventHandler<>), typeof(WorkItemAddedHandler<>)); // TODO: check why is not working ...

builder.Services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();

// Rules
builder.Services.AddSingleton<IRuleProcessor, RuleProcessor>();
var ruleDefinitions = builder.Configuration.GetSection("RuleDefinitions").Get<RuleDefinitions>();
builder.Services.AddSingleton(ruleDefinitions);

// Build
var app = builder.Build();
//app.Run(); //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//






// Load Test
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
    AccountId = new Guid("f6d6cde9-3e0e-4a7a-9081-efb972f9d0b2"),
    RequestingInvolvedPartyId = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6")
};

AccountBalanceController accountBalanceController = new AccountBalanceController();
accountBalanceController.GetBalance(accountBalanceCommandHandler, accountBalanceRequest, CancellationToken.None);


// -- Deposit --
//var accountBalanceCommandHandler = transactionCommandHandlerFactory.Create<DepositRequest, DepositResponse>();


/*
 
TransactionStatusTransitionValidator
ApprovalStatusTransitionValidator
ApprovalService call


get exchange rate service


jos jedan sloj izmedju entiteta i objekata u servisima

*/