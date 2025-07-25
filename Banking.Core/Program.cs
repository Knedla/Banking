using Banking.Core.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register Transactions
builder.Services.AddTransactionCommandWithDiscovery(typeof(Program).Assembly);

var app = builder.Build();