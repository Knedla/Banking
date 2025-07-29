using Banking.Application.Interfaces;
using Banking.Application.Notifications;
using Banking.Application.Notifications.Channels;
using Banking.Application.Notifications.Interfaces;
using Banking.Application.Notifications.Routing;
using Banking.Application.Notifications.Rules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API.Extension;

public static class NotificationRegistration
{
    public static IServiceCollection AddNotification(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<List<NotificationRule>>(config.GetSection("NotificationRules"));

        services.AddScoped(typeof(INotificationEngine<>), typeof(NotificationEngine<>));
        services.AddScoped<INotificationRuleProvider, NotificationRuleProvider>();
        services.AddScoped<INotificationDestinationResolver, NotificationDestinationResolver>();
        services.AddScoped<INotificationChannelDispatcher, NotificationChannelDispatcher>();

        // Channel mocks or real
        services.AddSingleton<IEmailSender, MockEmailSender>();
        services.AddSingleton<ISmsSender, MockSmsSender>();
        services.AddSingleton<IInAppNotifier, MockInAppNotifier>();

        services.AddScoped<ITemplateRenderer, SimpleTemplateRenderer>();

        return services;
    }
}
