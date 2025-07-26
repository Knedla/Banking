using Banking.API.Controllers;
using Banking.API.Extension;
using Banking.Application.Commands.Common;
using Banking.Application.Extensions;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;
using Banking.Infrastructure.Decorators;
using Banking.Infrastructure.Factories;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Stores;
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

// Register
builder.Services.AddSingleton<IBankingDataStore, InMemoryBankingDataStore>();
builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IExecutionTransactionCommandHandler<,>))
    .AddClasses(classes => classes.AssignableTo(typeof(IExecutionTransactionCommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.Decorate(
    typeof(IExecutionTransactionCommandHandler<,>),
    typeof(TransactionalExecutionCommandHandlerDecorator<,>));

// Bsuild
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