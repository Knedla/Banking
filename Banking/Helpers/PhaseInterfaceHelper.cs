using Banking.Commands.Transaction.Interfaces;
using Banking.Enumerations;

namespace Banking.Helpers
{
    public static class PhaseInterfaceHelper
    {
        public static Type ResolveType(CommandPhaseType phase, Type inputType, Type outputType)
        {
            return phase switch
            {
                CommandPhaseType.Initialization => typeof(IInitialization<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Validation => typeof(IValidation<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PreExecution => typeof(IPreExecution<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.Execution => typeof(IExecution<,>).MakeGenericType(inputType, outputType),
                CommandPhaseType.PostExecution => typeof(IPostExecution<,>).MakeGenericType(inputType, outputType),
                _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
            };
        }

        public static bool IsCommandPhaseInterface(Type iface)
        {
            return iface == typeof(IInitialization<,>) ||
                   iface == typeof(IValidation<,>) ||
                   iface == typeof(IPreExecution<,>) ||
                   iface == typeof(IExecution<,>) ||
                   iface == typeof(IPostExecution<,>);
        }

        public static CommandPhaseType GetPhaseFromInterface(Type iface)
        {
            if (iface == typeof(IInitialization<,>)) return CommandPhaseType.Initialization;
            if (iface == typeof(IValidation<,>)) return CommandPhaseType.Validation;
            if (iface == typeof(IPreExecution<,>)) return CommandPhaseType.PreExecution;
            if (iface == typeof(IExecution<,>)) return CommandPhaseType.Execution;
            if (iface == typeof(IPostExecution<,>)) return CommandPhaseType.PostExecution;
            throw new InvalidOperationException($"Unknown phase interface: {iface.Name}");
        }
    }
}
