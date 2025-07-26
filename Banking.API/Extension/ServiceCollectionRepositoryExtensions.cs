using Banking.Domain.Repositories;
using Banking.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions
{
    public static class ServiceCollectionRepositoryExtensions
    {
        public static IServiceCollection AddBankingInfrastructureRepository(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
