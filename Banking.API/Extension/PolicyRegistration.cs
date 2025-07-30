using Banking.Application.Interfaces;
using Banking.Application.Rules;
using Banking.Domain.Configuration;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.Rules;
using Banking.Infrastructure.Rules.TransactionApprovals;
using Banking.Infrastructure.Rules.TransactionFees;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API.Extension;

public static class PolicyRegistration
{
    public static IServiceCollection AddPolicy(this IServiceCollection services, IConfiguration config)
    {
        // Transacton Approvel
        var fourEyeSettings = config.GetSection("Transaction:ApprovalRules:FourEye").Get<FourEyeApprovalSettings>();
        var highValueSettings = config.GetSection("Transaction:ApprovalRules:HighValue").Get<HighValueApprovalSettings>();

        services.AddSingleton(fourEyeSettings);
        services.AddSingleton(highValueSettings);

        services.AddScoped<ITransactionApprovalRule, FourEyeApprovalRule>();
        services.AddScoped<ITransactionApprovalRule, HighValueTransactionApprovalRule>();

        // Fee
        var settings = config.GetSection("Transaction:FeeSettings").Get<TransactionFeeSettings>();

        services.AddSingleton(settings);

        services.AddScoped<ITransactionFeeRule, MinimumBalanceFeeRule>();
        services.AddScoped<ITransactionFeeRule, InternationalTransferFeeRule>();
        services.AddScoped<ITransactionFeeRule, StandardTransferFeeRule>();
        services.AddScoped<ITransactionFeeRule, ChannelFeeRule>();
        services.AddScoped<ITransactionFeeRule, WithdrawalFeeRule>();

        // Policies
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IPolicy))
            .AddClasses(classes => classes.AssignableTo(typeof(IPolicy)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Rules
        services.AddSingleton<IRuleProcessor, RuleProcessor>();

        var ruleDefinitions = config.GetSection("RuleDefinitions").Get<RuleDefinitions>();
        services.AddSingleton(ruleDefinitions);

        return services;
    }
}
