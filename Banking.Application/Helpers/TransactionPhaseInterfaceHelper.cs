using Banking.Application.Commands.Common;
using Banking.Application.Enumerations;

namespace Banking.Application.Helpers;

public static class TransactionPhaseInterfaceHelper
{
    public static Type ResolveType(TransactionCommandPhaseType phase, Type inputType, Type outputType)
    {
        return phase switch
        {
            TransactionCommandPhaseType.Initialization => typeof(IInitializationTransactionCommandHandler<,>).MakeGenericType(inputType, outputType),
            TransactionCommandPhaseType.Validation => typeof(IValidationTransactionCommandHandler<,>).MakeGenericType(inputType, outputType),
            TransactionCommandPhaseType.PreExecution => typeof(IPreExecutionTransactionCommandHandler<,>).MakeGenericType(inputType, outputType),
            TransactionCommandPhaseType.Execution => typeof(IExecutionTransactionCommandHandler<,>).MakeGenericType(inputType, outputType),
            TransactionCommandPhaseType.PostExecution => typeof(IPostExecutionTransactionCommandHandler<,>).MakeGenericType(inputType, outputType),
            _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
        };
    }

    public static bool IsCommandPhaseInterface(Type iface)
    {
        return iface == typeof(IInitializationTransactionCommandHandler<,>) ||
               iface == typeof(IValidationTransactionCommandHandler<,>) ||
               iface == typeof(IPreExecutionTransactionCommandHandler<,>) ||
               iface == typeof(IExecutionTransactionCommandHandler<,>) ||
               iface == typeof(IPostExecutionTransactionCommandHandler<,>);
    }

    public static TransactionCommandPhaseType GetPhaseFromInterface(Type iface)
    {
        if (iface == typeof(IInitializationTransactionCommandHandler<,>)) return TransactionCommandPhaseType.Initialization;
        if (iface == typeof(IValidationTransactionCommandHandler<,>)) return TransactionCommandPhaseType.Validation;
        if (iface == typeof(IPreExecutionTransactionCommandHandler<,>)) return TransactionCommandPhaseType.PreExecution;
        if (iface == typeof(IExecutionTransactionCommandHandler<,>)) return TransactionCommandPhaseType.Execution;
        if (iface == typeof(IPostExecutionTransactionCommandHandler<,>)) return TransactionCommandPhaseType.PostExecution;
        throw new InvalidOperationException($"Unknown phase interface: {iface.Name}");
    }
}
