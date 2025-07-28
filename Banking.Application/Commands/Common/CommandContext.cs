using System.Diagnostics;

namespace Banking.Application.Commands.Common;

public class CommandContext<TInput, TOutput>
{
    public TInput Input { get; }
    public TOutput Output { get; set; }
    public IDictionary<string, object> Data { get; } = new Dictionary<string, object>();
    public List<string> Logs { get; } = new();
    public Stopwatch TotalStopwatch { get; } = Stopwatch.StartNew();

    public CommandContext(TInput input)
    {
        Input = input;
    }

    public void Log(string message)
    {
        Logs.Add($"[{DateTime.UtcNow:HH:mm:ss.fff}] {message}");
    }

    public T? Get<T>(string key)
        => Data.TryGetValue(key, out var val) && val is T t ? t : default;

    public void Set<T>(string key, T value)
        => Data[key] = value!;
}
