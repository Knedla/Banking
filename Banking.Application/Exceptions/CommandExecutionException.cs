using Banking.Application.Enumerations;

namespace Banking.Application.Exceptions
{
    public class CommandExecutionException : Exception
    {
        public CommandPhaseType Phase { get; }
        public Type CommandType { get; }

        public CommandExecutionException(
            string message,
            CommandPhaseType phase,
            Type commandType,
            Exception? innerException = null)
            : base(FormatMessage(message, phase, commandType, innerException), innerException)
        {
            Phase = phase;
            CommandType = commandType;
        }

        private static string FormatMessage(string baseMessage, CommandPhaseType phase, Type commandType, Exception? inner)
        {
            var detail = $"{baseMessage} [Phase: {phase}, Command: {commandType.Name}]";
            return inner != null ? $"{detail} -> {inner.Message}" : detail;
        }
    }
}
