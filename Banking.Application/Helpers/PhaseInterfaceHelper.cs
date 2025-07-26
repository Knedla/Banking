using Banking.Application.Commands.Common;
using Banking.Application.Enumerations;

namespace Banking.Application.Helpers
{
    public static class PhaseInterfaceHelper
    {
        public static Type ResolveType(CommandPhaseType phase, Type inputType, Type outputType)
        {
            return phase switch
            {
                CommandPhaseType.Initialization => typeof(IInitializationCommandHandler<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Validation => typeof(IValidationCommandHandler<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PreExecution => typeof(IPreExecutionCommandHandler<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Execution => typeof(IExecutionCommandHandler<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PostExecution => typeof(IPostExecutionCommandHandler<,>).MakeGenericType(inputType, outputType),
                _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
            };
        }

        public static bool IsCommandPhaseInterface(Type iface)
        {
            return iface == typeof(IInitializationCommandHandler<,>) ||
                   iface == typeof(IValidationCommandHandler<,>) ||
                   iface == typeof(IPreExecutionCommandHandler<,>) ||
                   iface == typeof(IExecutionCommandHandler<,>) ||
                   iface == typeof(IPostExecutionCommandHandler<,>);
        }

        public static CommandPhaseType GetPhaseFromInterface(Type iface)
        {
            if (iface == typeof(IInitializationCommandHandler<,>)) return CommandPhaseType.Initialization;
            if (iface == typeof(IValidationCommandHandler<,>)) return CommandPhaseType.Validation;
            if (iface == typeof(IPreExecutionCommandHandler<,>)) return CommandPhaseType.PreExecution;
            if (iface == typeof(IExecutionCommandHandler<,>)) return CommandPhaseType.Execution;
            if (iface == typeof(IPostExecutionCommandHandler<,>)) return CommandPhaseType.PostExecution;
            throw new InvalidOperationException($"Unknown phase interface: {iface.Name}");
        }
    }
}
