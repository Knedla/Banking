using Banking.Domain.Configuration;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.Policies;
using Banking.Infrastructure.Rules.TransactionFees;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API.Extension;

public static class TransactionFeePolicyRegistration
{
    public static IServiceCollection AddTransactionFeePolicy(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection("Transaction:FeeSettings").Get<TransactionFeeSettings>();

        services.AddSingleton(settings);

        services.AddScoped<ITransactionFeeRule, MinimumBalanceFeeRule>();
        services.AddScoped<ITransactionFeeRule, InternationalTransferFeeRule>();
        services.AddScoped<ITransactionFeeRule, StandardTransferFeeRule>();
        services.AddScoped<ITransactionFeeRule, ChannelFeeRule>();
        services.AddScoped<ITransactionFeeRule, WithdrawalFeeRule>();

        services.AddScoped<TransactionFeePolicy>();

        return services;
    }
}
