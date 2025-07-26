using Banking.API.Controllers;
using Banking.API.Extension;
using Banking.Application.Commands.Common;
using Banking.Application.Extensions;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// ServiceCollectionRepositoryExtensions
builder.Services.AddBankingInfrastructureRepository();

// Register Services
builder.Services.AddBankingInfrastructureServices();

// Register CommandHandlers
builder.Services.AddBankingApplicationTransactionCommandHandlersWithDiscovery(typeof(ICommandHandler<,>).Assembly);

// Register Factory
builder.Services.AddScoped<ITransactionCommandHandlerFactory, TransactionCommandHandlerFactory>();

// Bsuild
var app = builder.Build();
//app.Run(); //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//






// Test
var transactionCommandHandlerFactory = app.Services.GetService(typeof(ITransactionCommandHandlerFactory)) as ITransactionCommandHandlerFactory;
var commandHandler = transactionCommandHandlerFactory.Create<AccountBalanceRequest, AccountBalanceResponse>();

//app.Services.GetService(typeof(ICommandHandler<AccountBalanceRequest, AccountBalanceResponse>)) as ICommandHandler<AccountBalanceRequest, AccountBalanceResponse>;
var request = new AccountBalanceRequest()
{
    AccountId = Guid.NewGuid(),
    RequestingUserId = Guid.NewGuid()
};

AccountBalanceController accountBalanceController = new AccountBalanceController();
accountBalanceController.GetBalance(commandHandler, request, CancellationToken.None);