using Banking.Application.Interfaces.Services;
using Banking.Application.Policies;
using Banking.Application.Services;
using Banking.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Banking.Infrastructure
        services.AddScoped<ICreateAccountRequestService, CreateAccountRequestService>();
        services.AddScoped<ICreateAccountService, CreateAccountService>();
        services.AddScoped<IGetBalanceService, GetBalanceService>();
        services.AddScoped<IDepositService, DepositService>();
        services.AddScoped<IWithdrawalService, WithdrawalService>();
        services.AddScoped<ITransferService, TransferService>();
        services.AddScoped<IInvolvedPartyService, InvolvedPartyService>();
        services.AddScoped<IFraudDetectionService, FraudDetectionService>();
        services.AddScoped<IUserContextService, UserContextService> ();
        services.AddScoped<ITransactionApprovalService, TransactionApprovalService>();
        services.AddScoped<ITransactionFeeService, TransactionFeeService>();
        services.AddScoped(typeof(IPolicyService<>), typeof(PolicyService<>));
        services.AddScoped<IUpdateBalanceService, UpdateBalanceService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        return services;
    }
}
