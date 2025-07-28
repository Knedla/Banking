using Banking.Domain.Configuration;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.Policies;
using Banking.Infrastructure.Rules.TransactionApprovals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API.Extension;

public static class TransactionApprovalPolicyRegistration
{
    public static IServiceCollection AddTransactionApprovalPolicy(this IServiceCollection services, IConfiguration config)
    {
        var fourEyeSettings = config.GetSection("Transaction:ApprovalRules:FourEye").Get<FourEyeApprovalSettings>();
        var highValueSettings = config.GetSection("Transaction:ApprovalRules:HighValue").Get<HighValueApprovalSettings>();

        services.AddSingleton(fourEyeSettings);
        services.AddSingleton(highValueSettings);

        services.AddScoped<ITransactionApprovalRule, FourEyeApprovalRule>();
        services.AddScoped<ITransactionApprovalRule, HighValueTransactionApprovalRule>();

        services.AddScoped<TransactionApprovalPolicy>();

        return services;
    }
}
