using Banking.Domain.Repositories;
using Banking.Infrastructure.Persistence.Repositories;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddSingleton<IBankingDataStore, InMemoryBankingDataStore>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

        return services;
    }
}
