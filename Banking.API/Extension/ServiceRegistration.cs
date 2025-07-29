using Banking.Application.Interfaces.Services;
using Banking.Application.Services.Approval;
using Banking.Domain.Interfaces.Services;
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

        // Banking.Application
        services.AddScoped<IApprovalService, ApprovalService>();

        return services;
    }
}
