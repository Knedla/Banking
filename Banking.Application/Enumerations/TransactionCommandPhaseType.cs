namespace Banking.Application.Enumerations;

// do not forget to update TransactionPhaseInterfaceHelper
public enum TransactionCommandPhaseType
{
    Initialization = 0,
    Validation = 1,
    PreExecution = 2,
    Execution = 3, // required - can be changed...
    PostExecution = 4
}
