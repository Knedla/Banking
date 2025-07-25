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
                CommandPhaseType.Initialization => typeof(IInitializationCommand<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Validation => typeof(IValidationCommand<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PreExecution => typeof(IPreExecutionCommand<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Execution => typeof(IExecutionCommand<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PostExecution => typeof(IPostExecutionCommand<,>).MakeGenericType(inputType, outputType),
                _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
            };
        }

        public static bool IsCommandPhaseInterface(Type iface)
        {
            return iface == typeof(IInitializationCommand<,>) ||
                   iface == typeof(IValidationCommand<,>) ||
                   iface == typeof(IPreExecutionCommand<,>) ||
                   iface == typeof(IExecutionCommand<,>) ||
                   iface == typeof(IPostExecutionCommand<,>);
        }

        public static CommandPhaseType GetPhaseFromInterface(Type iface)
        {
            if (iface == typeof(IInitializationCommand<,>)) return CommandPhaseType.Initialization;
            if (iface == typeof(IValidationCommand<,>)) return CommandPhaseType.Validation;
            if (iface == typeof(IPreExecutionCommand<,>)) return CommandPhaseType.PreExecution;
            if (iface == typeof(IExecutionCommand<,>)) return CommandPhaseType.Execution;
            if (iface == typeof(IPostExecutionCommand<,>)) return CommandPhaseType.PostExecution;
            throw new InvalidOperationException($"Unknown phase interface: {iface.Name}");
        }
    }
}
