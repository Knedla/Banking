using Banking.Application.Attributes;
using Banking.Application.Commands.Common;
using Banking.Application.Enumerations;
using Banking.Application.Exceptions;
using Banking.Application.Helpers;
using Banking.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Banking.Core.Commands.Transaction
{
    public class TransactionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput>
    {
        private readonly ILogger<TransactionCommandHandler<TInput, TOutput>>? _logger;
        private readonly ITelemetryService? _telemetry;

        private readonly List<(CommandPhaseType Phase, ICommandHandler<TInput, TOutput> Command)> _ordered;

        public TransactionCommandHandler(
            IServiceProvider provider,
            ILogger<TransactionCommandHandler<TInput, TOutput>>? logger = null,
            ITelemetryService? telemetry = null)
        {
            _logger = logger;
            _telemetry = telemetry;
            _ordered = new();

            var phaseOrder = Enum.GetValues<CommandPhaseType>().ToList();

            foreach (var phase in phaseOrder)
            {
                var iface = PhaseInterfaceHelper.ResolveType(phase, typeof(TInput), typeof(TOutput));
                var commands = provider.GetServices(iface).Cast<ICommandHandler<TInput, TOutput>>();

                var ordered = commands
                    .Select(cmd => (
                        Phase: phase,
                        Command: cmd,
                        Order: cmd.GetType().GetCustomAttribute<PhaseOrderAttribute>()?.Order ?? int.MaxValue
                    ))
                    .OrderBy(x => x.Order)
                    .Select(x => (x.Phase, x.Command));

                _ordered.AddRange(ordered);
            }

            // Ensure at least one IExecution is registered
            if (!_ordered.Any(x => x.Phase == CommandPhaseType.Execution))
                throw new InvalidOperationException($"No {nameof(IExecutionCommandHandler<TInput, TOutput>)} command registered.");
        }

        public async Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            // Always return true for TransactionCommandHandler; each child will decide its own CanExecute
            return true;
        }

        public async Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            foreach (var (phase, command) in _ordered)
            {
                if (!await command.CanExecuteAsync(context, ct)) continue;

                int attempts = 0;
                Exception? lastException = null;

                do
                {
                    try
                    {
                        context.Log($"[{phase}] Executing {command.GetType().Name} (attempt {attempts + 1})...");
                        var stopwatch = Stopwatch.StartNew();

                        await command.ExecuteAsync(context, ct);

                        stopwatch.Stop();
                        context.Log($"[{phase}] {command.GetType().Name} completed in {stopwatch.ElapsedMilliseconds}ms");
                        lastException = null;

                        break;
                    }
                    catch (Exception ex)
                    {
                        attempts++;
                        lastException = ex;

                        context.Log($"[{phase}] {command.GetType().Name} failed after {attempts} attempts: {ex.Message}");
                        throw new CommandExecutionException("Command failed", phase, command.GetType(), ex);
                    }
                } while (lastException != null);
            }

            context.Log($"Transaction completed in {context.TotalStopwatch.ElapsedMilliseconds}ms");
        }
    }
}