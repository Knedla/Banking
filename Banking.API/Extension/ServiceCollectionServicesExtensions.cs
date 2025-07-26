using Banking.Application.Interfaces.Services;
using Banking.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions
{
    public static class ServiceCollectionServicesExtensions
    {
        public static IServiceCollection AddBankingInfrastructureServices(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<ICreateAccountService, CreateAccountService>();
            services.AddScoped<IGetBalanceService, GetBalanceService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<IWithdrawalService, WithdrawalService>();
            services.AddScoped<ITransferService, TransferService>();

            return services;
        }
    }
}
