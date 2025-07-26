using System.Reflection;
using Banking.Application.Attributes;
using Banking.Application.Commands.Common;
using Banking.Application.Enumerations;
using Banking.Application.Helpers;
using Banking.Application.Interfaces.Factories;
using Banking.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Infrastructure.Factories
{
    public class TransactionCommandHandlerFactory : ITransactionCommandHandlerFactory
    {
        private readonly IServiceProvider _provider;

        public TransactionCommandHandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ICommandHandler<TInput, TOutput> Create<TInput, TOutput>()
        {
            var logger = _provider.GetService<ILoggerService>();
            var telemetry = _provider.GetService<ITelemetryService>();

            var inputType = typeof(TInput);
            var outputType = typeof(TOutput);

            var orderedHandlers = new List<ITransactionCommandHandler<TInput, TOutput>>();

            foreach (var phase in Enum.GetValues<TransactionCommandPhaseType>())
            {
                var iface = TransactionPhaseInterfaceHelper.ResolveType(phase, inputType, outputType);

                var handlers = _provider.GetServices(iface)
                    .Cast<ITransactionCommandHandler<TInput, TOutput>>()
                    .Select(handler => new
                    {
                        Handler = handler,
                        Order = handler.GetType().GetCustomAttribute<PhaseOrderAttribute>()?.Order ?? int.MaxValue
                    })
                    .OrderBy(x => x.Order)
                    .Select(x => x.Handler);

                orderedHandlers.AddRange(handlers);
            }

            return new TransactionCommandHandler<TInput, TOutput>(orderedHandlers, logger, telemetry);
        }
    }
}
