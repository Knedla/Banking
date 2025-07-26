using Banking.Application.Interfaces.Services;
using System.Diagnostics;

namespace Banking.Application.Commands.Common
{
    public class TransactionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput>
    {
        private readonly ILoggerService? _logger;
        private readonly ITelemetryService? _telemetry;
        private readonly IReadOnlyList<ITransactionCommandHandler<TInput, TOutput>> _commands;

        public TransactionCommandHandler(
            IEnumerable<ITransactionCommandHandler<TInput, TOutput>> commands,
            ILoggerService? logger = null,
            ITelemetryService? telemetry = null)
        {
            _logger = logger;
            _telemetry = telemetry;
            _commands = commands.ToList();
        }

        public Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken ct)
        {
            foreach (var command in _commands)
            {
                if (!await command.CanExecuteAsync(context, ct))
                    return; // break and return, do not continue

                try
                {
                    context.Log($"Executing {command.GetType().Name}...");
                    var stopwatch = Stopwatch.StartNew(); // move to some config so it can be turned on and off

                    await command.ExecuteAsync(context, ct);

                    stopwatch.Stop();
                    context.Log($"{command.GetType().Name} completed in {stopwatch.ElapsedMilliseconds}ms");
                }
                catch (Exception ex)
                {
                    context.Log($"{command.GetType().Name} failed: {ex.Message}");
                    throw;
                }
            }

            context.Log($"Transaction completed in {context.TotalStopwatch.ElapsedMilliseconds}ms");
        }
    }
}
