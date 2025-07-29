using Banking.Domain.Repositories;
using Banking.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();

        return services;
    }
}
