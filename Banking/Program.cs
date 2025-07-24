using Banking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register services
builder.Services.AddTransient<TestRunService>();

var app = builder.Build();

// Resolve and run your service
var testRunService = app.Services.GetRequiredService<TestRunService>();
testRunService.Run();
