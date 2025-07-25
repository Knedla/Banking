using Banking.Core.Commands.Interfaces;
using Banking.Core.Commands.Transaction;
using Banking.Core.Enumerations;
using Banking.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Banking.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Dictionary<(Type Input, Type Output), List<(CommandPhaseType Phase, Type CommandType)>> _cache = new();
        public static IServiceCollection AddTransactionCommandWithDiscovery(
            this IServiceCollection services,
            Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var type in types)
            {
                var iface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && PhaseInterfaceHelper.IsCommandPhaseInterface(i.GetGenericTypeDefinition()));

                if (iface == null)
                    continue;

                var genericArgs = iface.GetGenericArguments();
                var input = genericArgs[0];
                var output = genericArgs[1];
                var phase = PhaseInterfaceHelper.GetPhaseFromInterface(iface.GetGenericTypeDefinition());

                var key = (Input: input, Output: output);
                if (!_cache.TryGetValue(key, out var list))
                {
                    list = new();
                    _cache[key] = list;
                }

                list.Add((phase, type));
                services.AddTransient(iface, type);
            }

            foreach (var ((input, output), commands) in _cache)
            {
                var executionRegistered = commands.Any(x => x.Phase == CommandPhaseType.Execution);
                if (!executionRegistered)
                {
                    throw new InvalidOperationException(
                        $"No IExecution<{input.Name}, {output.Name}> registered for transaction pipeline.");
                }

                var cmdType = typeof(TransactionCommand<,>).MakeGenericType(input, output);
                var icmdType = typeof(ICommand<,>).MakeGenericType(input, output);

                services.AddTransient(icmdType, cmdType);
            }

            return services;
        }
    }
}
