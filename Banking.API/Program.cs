using Banking.API.Controllers;
using Banking.API.Extension;
using Banking.Application.Extensions;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Validators.StateMachine;
using Banking.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddEvents();
builder.Services.AddRepository();
builder.Services.AddServices();
builder.Services.AddTransactionCommands();
builder.Services.AddNotification(builder.Configuration);
builder.Services.AddPolicy(builder.Configuration);

// StateMachine
builder.Services.AddSingleton<ITransactionStateValidator, TransactionStatusValidator>();
builder.Services.AddSingleton<IStateValidator<ApprovalStatus>, ApprovalStatusTransitionValidator>();
builder.Services.AddSingleton<IStateValidator<WorkItemStatus>, WorkItemStatusTransitionValidator>();

// Account
builder.Services.AddSingleton<IAccountNumberGenerator, AccountNumberGenerator>();
builder.Services.AddSingleton<ICurrencyExchangeService, CurrencyExchangeService>(); // TODO: must have a period/time of validity, the exchange rate changes on a daily basis
builder.Services.AddSingleton<IAccountBalanceFactory, AccountBalanceFactory>();

// Build
var app = builder.Build();
//app.Run();






// Load Test
var transactionCommandHandlerFactory = app.Services.GetService(typeof(ITransactionCommandHandlerFactory)) as ITransactionCommandHandlerFactory;

// -- CreateAccount --
var createAccountCommandHandler = transactionCommandHandlerFactory.Create<CreateAccountRequestRequest, CreateAccountRequestResponse>();
var createAccountRequest = new CreateAccountRequestRequest()
{
    UserId = Guid.NewGuid(),
    InvolvedPartyId = new Guid("f4bd0b90-10c6-47ff-bb35-113c0c779c3f"),
    AccountType = AccountType.Standard,
    FromCurrencyCode = "USD",
    ToCurrencyCode = "EUR",
    InitialDeposit = 500,
    TransactionChannel = TransactionChannel.Branch
};

CreateAccountController createAccountController = new CreateAccountController();
createAccountController.CreateAccount(createAccountCommandHandler, createAccountRequest, CancellationToken.None);


// -- AccountBalance --
var accountBalanceCommandHandler = transactionCommandHandlerFactory.Create<AccountBalanceRequest, AccountBalanceResponse>();

var accountBalanceRequest = new AccountBalanceRequest()
{
    AccountId = new Guid("f6d6cde9-3e0e-4a7a-9081-efb972f9d0b2"),
    InvolvedPartyId = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6")
};

AccountBalanceController accountBalanceController = new AccountBalanceController();
accountBalanceController.GetBalance(accountBalanceCommandHandler, accountBalanceRequest, CancellationToken.None);


// -- Deposit --
var depositCommandHandler = transactionCommandHandlerFactory.Create<DepositRequest, DepositResponse>();

var depositRequest = new DepositRequest()
{
    UserId = Guid.NewGuid(),
    TransactionAccountDetails = new TransactionAccountDetails()
    {
        AccountId = new Guid("f6d6cde9-3e0e-4a7a-9081-efb972f9d0b2")
    },
    TransactionInitializedById = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6"),
    CurrencyCode = "EUR",
    Amount = 200,
    TransactionChannel = TransactionChannel.ATM
};

DepositController depositController = new DepositController();
depositController.Deposit(depositCommandHandler, depositRequest, CancellationToken.None);


// -- Withdrawal --
var withdrawalCommandHandler = transactionCommandHandlerFactory.Create<WithdrawalRequest, WithdrawalResponse>();

var withdrawalRequest = new WithdrawalRequest()
{
    UserId = Guid.NewGuid(),
    TransactionAccountDetails = new TransactionAccountDetails()
    {
        AccountId = new Guid("b67dc798-e95f-464a-bbd1-f56b57d60a5e")
    },
    TransactionInitializedById = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6"),
    CurrencyCode = "EUR",
    Amount = 400,
    TransactionChannel = TransactionChannel.ATM
};

WithdrawalController withdrawalController = new WithdrawalController();
withdrawalController.Withdraw(withdrawalCommandHandler, withdrawalRequest, CancellationToken.None);


// -- Transfer --
var transferCommandHandler = transactionCommandHandlerFactory.Create<TransferRequest, TransferResponse>();

var transferRequest = new TransferRequest()
{
    UserId = Guid.NewGuid(),
    TransactionInitializedById = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6"),
    FromTransactionAccountDetails = new TransactionAccountDetails()
    {
        AccountId = new Guid("f6d6cde9-3e0e-4a7a-9081-efb972f9d0b2")
    },
    CounterpartyAccountDetails = new CounterpartyAccountDetails()
    {
        PaymentReference = "something"
    },
    ToTransactionAccountDetails = new TransactionAccountDetails()
    {
        AccountId = new Guid("b67dc798-e95f-464a-bbd1-f56b57d60a5e")
    },
    
    FromCurrencyCode = "EUR",
    Amount = 1000,
    TransactionChannel = TransactionChannel.MobileApp
};

TransferController transferController = new TransferController();
transferController.Transfer(transferCommandHandler, transferRequest, CancellationToken.None);
