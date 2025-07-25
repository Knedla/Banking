using Banking.Attributes;
using Banking.Commands.Interfaces;
using Banking.Commands.Transaction.Interfaces;
using Banking.Enumerations;
using Banking.Exceptions;
using Banking.Helpers;
using Banking.Policies;
using Banking.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Banking.Commands.Transaction
{
    public class TransactionCommand<TInput, TOutput> : ICommand<TInput, TOutput>
    {
        private readonly ILogger<TransactionCommand<TInput, TOutput>>? _logger;
        private readonly ITelemetryService? _telemetry;

        private readonly List<(CommandPhaseType Phase, ICommand<TInput, TOutput> Command)> _ordered;

        public TransactionCommand(
            IServiceProvider provider,
            ILogger<TransactionCommand<TInput, TOutput>>? logger = null,
            ITelemetryService? telemetry = null)
        {
            _logger = logger;
            _telemetry = telemetry;
            _ordered = new();

            var phaseOrder = Enum.GetValues<CommandPhaseType>().ToList();

            foreach (var phase in phaseOrder)
            {
                var iface = PhaseInterfaceHelper.ResolveType(phase, typeof(TInput), typeof(TOutput));
                var commands = provider.GetServices(iface).Cast<ICommand<TInput, TOutput>>();

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
                throw new InvalidOperationException($"No {nameof(IExecution<TInput, TOutput>)} command registered.");
        }

        public async Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            // Always return true for TransactionCommand; each child will decide its own CanExecute
            return true;
        }

        public async Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            foreach (var (phase, command) in _ordered)
            {
                if (!await command.CanExecuteAsync(context, ct)) continue;

                var policy = RetryPolicyResolver.GetPolicy(command.GetType());

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

                        if (policy == null || attempts > policy.MaxRetries)
                        {
                            context.Log($"[{phase}] {command.GetType().Name} failed after {attempts} attempts: {ex.Message}");
                            throw new CommandExecutionException("Command failed", phase, command.GetType(), ex);
                        }

                        var delay = policy.Exponential
                            ? TimeSpan.FromSeconds(policy.Delay.TotalSeconds * Math.Pow(2, attempts - 1))
                            : policy.Delay;

                        context.Log($"[{phase}] Retrying {command.GetType().Name} after {delay.TotalSeconds}s (attempt {attempts})...");
                        await Task.Delay(delay, ct);
                    }
                } while (lastException != null);
            }

            context.Log($"Transaction completed in {context.TotalStopwatch.ElapsedMilliseconds}ms");
        }
    }
}
