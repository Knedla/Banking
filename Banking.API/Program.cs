using Banking.Application.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register Transactions
builder.Services.AddTransactionCommandHandlerWithDiscovery(typeof(Program).Assembly); // change to Banking.Business

var app = builder.Build();