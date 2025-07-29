using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Factories;
using Banking.Infrastructure.Decorators;
using Banking.Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API.Extension;

public static class TransactionCommandsRegistration
{
    public static IServiceCollection AddTransactionCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IInitializationTransactionCommandHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(IInitializationTransactionCommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IValidationTransactionCommandHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(IValidationTransactionCommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IPreExecutionTransactionCommandHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(IPreExecutionTransactionCommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IExecutionTransactionCommandHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(IExecutionTransactionCommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IPostExecutionTransactionCommandHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(IPostExecutionTransactionCommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(
            typeof(IExecutionTransactionCommandHandler<,>),
            typeof(TransactionalExecutionCommandHandlerDecorator<,>));

        services.AddScoped<ITransactionCommandHandlerFactory, TransactionCommandHandlerFactory>();

        return services;
    }
}
